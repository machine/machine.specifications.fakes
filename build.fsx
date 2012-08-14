#I @"Source\packages\Fake.1.64.6\tools"
#r "FakeLib.dll"
#r "System.Web.Extensions.dll"

open Fake
open Fake.Git
open System.Collections.Generic
open System.Linq
open System.IO
open System.Web.Script.Serialization

(* properties *)
let authors = ["The machine project"]
let projectName = "Machine.Fakes"
let copyright = "Copyright - Machine.Fakes 2011 - 2012"
let NugetKey = if System.IO.File.Exists @".\key.txt" then ReadFileAsString @".\key.txt" else ""

let version =
    if hasBuildParam "version" then getBuildParam "version" else
    if isLocalBuild then getLastTag() else
    // version is set to the last tag retrieved from GitHub Rest API
    // see http://developer.github.com/v3/repos/ for reference
    let url = "https://api.github.com/repos/machine/machine.fakes/tags"
    tracefn "Downloading tags from %s" url
    let tagsFile = REST.ExecuteGetCommand null null url
    let tags = (new JavaScriptSerializer()).DeserializeObject(tagsFile) :?> System.Object array
    [ for tag in tags -> tag :?> Dictionary<string, System.Object> ]
        |> List.map (fun m -> m.Item("name") :?> string)
        |> List.max

let title = if isLocalBuild then sprintf "%s (%s)" projectName <| getCurrentHash() else projectName


(* Directories *)
let buildDir = @".\Build\"
let packagesDir = @".\Source\packages\"
let docsDir = buildDir + @"Documentation\"
let testOutputDir = buildDir + @"Specs\"
let nugetDir = buildDir + @"NuGet\"
let testDir = buildDir
let deployDir = @".\Release\"
let targetPlatformDir = getTargetPlatformDir "v4.0.30319"
let nugetLibDir = nugetDir + @"lib\net40\"

(* files *)
let slnReferences = !! @".\Source\*.sln"

(* flavours *)
let Flavours = ["RhinoMocks"; "FakeItEasy"; "NSubstitute"; "Moq"]
let MSpecVersion() = GetPackageVersion packagesDir "Machine.Specifications"
let mspecTool() = sprintf @".\Source\packages\Machine.Specifications.%s\tools\mspec-clr4.exe" (MSpecVersion())

(* Targets *)
Target "Clean" (fun _ -> CleanDirs [buildDir; testDir; deployDir; docsDir; testOutputDir] )


Target "BuildApp" (fun _ ->
    AssemblyInfo
      (fun p ->
        {p with
            CodeLanguage = CSharp;
            AssemblyVersion = version;
            AssemblyTitle = title;
            AssemblyDescription = "An integration layer for fake frameworks on top of MSpec";
            AssemblyCopyright = copyright;
            Guid = "3745F3DA-6ABB-4C58-923D-B09E4A04688F";
            OutputFileName = @".\Source\GlobalAssemblyInfo.cs"})

    slnReferences
        |> MSBuildRelease buildDir "Build"
        |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
    ActivateFinalTarget "DeployTestResults"
    !+ (testDir + "/*.Specs.dll")
      ++ (testDir + "/*.Examples.dll")
        |> Scan
        |> MSpec (fun p ->
                    {p with
                        ToolPath = mspecTool()
                        HtmlOutputDir = testOutputDir})
)


Target "MergeStructureMap" (fun _ ->
    Rename (buildDir + "Machine.Fakes.Partial.dll") (buildDir + "Machine.Fakes.dll")

    ILMerge
        (fun p ->
            {p with
                Libraries =
                    [buildDir + "StructureMap.dll"
                     buildDir + "StructureMap.AutoMocking.dll"]
                Internalize = InternalizeExcept "ILMergeExcludes.txt"
                TargetPlatform = sprintf @"v4,%s" targetPlatformDir})

        (buildDir + "Machine.Fakes.dll")
        (buildDir + "Machine.Fakes.Partial.dll")

    ["StructureMap.dll";
     "StructureMap.pdb";
     "StructureMap.xml";
     "StructureMap.AutoMocking.dll";
     "StructureMap.AutoMocking.xml";
     "Machine.Fakes.Partial.dll"]
        |> Seq.map (fun a -> buildDir + a)
        |> DeleteFiles
)

FinalTarget "DeployTestResults" (fun () ->
    !+ (testOutputDir + "\**\*.*")
      |> Scan
      |> Zip testOutputDir (sprintf "%sMSpecResults.zip" deployDir)
)

Target "GenerateDocumentation" (fun _ ->
    !+ (buildDir + "Machine.Fakes.dll")
      |> Scan
      |> Docu (fun p ->
          {p with
              ToolPath = "./tools/docu/docu.exe"
              TemplatesPath = "./tools/docu/templates"
              OutputPath = docsDir })
)

Target "ZipDocumentation" (fun _ ->
    !! (docsDir + "/**/*.*")
      |> Zip docsDir (deployDir + sprintf "Documentation-%s.zip" version)
)

Target "BuildZip" (fun _ ->
    !+ (buildDir + "/**/*.*")
      -- "*.zip"
      -- "**/*.Specs.*"
        |> Scan
        |> Zip buildDir (deployDir + sprintf "%s-%s.zip" projectName version)
)

let RequireAtLeast version = sprintf "%s" <| NormalizeVersion version

Target "BuildNuGet" (fun _ ->
    CleanDirs [nugetDir; nugetLibDir]

    [buildDir + "Machine.Fakes.dll"]
        |> CopyTo nugetLibDir
    
    NuGet (fun p ->
        {p with
            Summary = "A framework for faking dependencies on top of Machine.Specifications."
            Authors = authors
            Project = projectName
            Version = version
            OutputPath = nugetDir
            Dependencies = ["Machine.Specifications",RequireAtLeast (MSpecVersion())]
            AccessKey = NugetKey
            Publish = NugetKey <> "" })
        "machine.fakes.nuspec"

    !! (nugetDir + "Machine.Fakes.*.nupkg")
      |> CopyTo deployDir
)

Target "BuildNuGetFlavours" (fun _ ->
    Flavours
      |> Seq.iter (fun (flavour) ->
            let flavourVersion = GetPackageVersion packagesDir flavour
            CleanDirs [nugetDir; nugetLibDir]

            [buildDir + sprintf "Machine.Fakes.Adapters.%s.dll" flavour]
              |> CopyTo nugetLibDir

            NuGet (fun p ->
                {p with
                    Summary = sprintf "A framework for faking objects with %s on top of Machine.Specifications." flavour
                    Authors = authors
                    Project = sprintf "%s.%s" projectName flavour
                    Description = sprintf " This is the adapter for %s %s" flavour flavourVersion
                    Version = version
                    OutputPath = nugetDir
                    Dependencies =
                        ["Machine.Fakes",RequireExactly (NormalizeVersion version)
                         flavour,RequireAtLeast flavourVersion]
                    AccessKey = NugetKey
                    Publish = NugetKey <> "" })
                "machine.fakes.nuspec"

            !! (nugetDir + sprintf "Machine.Fakes.%s.*.nupkg" flavour)
              |> CopyTo deployDir)
)

Target "Default" DoNothing
Target "Deploy" DoNothing

// Build order
"Clean"
  ==> "BuildApp"
  ==> "Test"
  ==> "MergeStructureMap"
  ==> "BuildZip"
  ==> "GenerateDocumentation"
  ==> "ZipDocumentation"
  ==> "BuildNuGet"
  ==> "BuildNuGetFlavours"
  ==> "Deploy"
  ==> "Default"

// start build
RunParameterTargetOrDefault  "target" "Default"
