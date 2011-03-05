#I "tools/FAKE"
#r "FakeLib.dll"

open Fake
open Fake.Git
open System.Linq
open System.Text.RegularExpressions

(* properties *)
let authors = ["Bjoern Rochel"]
let projectName = "Machine.Fakes"
let copyright = "Copyright - Machine.Fakes 2011"
let versionFile = "version.txt"
let version = 
    if isLocalBuild then getLastTag() else
    // version is set to the last tag retrieved from GitHub Rest API
    let url = "http://github.com/api/v2/json/repos/show/BjRo/Machine.Fakes/tags"
    tracefn "Downloading tags from %s" url
    let tagsFile = REST.ExecuteGetCommand null null url
    let r = new Regex("[,{][\"]([^\"]*)[\"]")
    [for m in r.Matches tagsFile -> m.Groups.[1]]
        |> List.map (fun m -> m.Value)
        |> List.filter ((<>) "tags")
        |> List.max

let title = if isLocalBuild then sprintf "%s (%s)" projectName <| getCurrentHash() else projectName

(* flavours *)
let flavours = 
    ["RhinoMocks","3.6"; 
     "FakeItEasy","1.6.4062.205";
     "NSubstitute","1.0.0.0";
     "Moq","4.0.10827"]

(* Directories *)
let buildDir = @".\Build\"
let docsDir = buildDir + @"Documentation\"
let testOutputDir = buildDir + @"Specs\"
let nugetDir = buildDir + @"NuGet\" 
let testDir = buildDir
let deployDir = @".\Release\"
let targetPlatformDir = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319"
let nugetDocsDir = nugetDir + @"docs\"
let nugetLibDir = nugetDir + @"lib\"

(* files *)
let appReferences = !! @".\Source\**\*.csproj"

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

    appReferences
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

Target "BuildNuGet" (fun _ ->
    CleanDirs [nugetDir; nugetLibDir; nugetDocsDir]
        
    XCopy docsDir nugetDocsDir
    [buildDir + "Machine.Fakes.dll"]
        |> CopyTo nugetLibDir

    NuGet (fun p -> 
        {p with               
            Authors = authors
            Project = projectName
            Version = version                        
            OutputPath = nugetDir
            Dependencies = ["Machine.Specifications","0.3.0.0"]
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey" })
        "machine.fakes.nuspec"

    [nugetDir + sprintf "Machine.Fakes.%s.nupkg" version]
        |> CopyTo deployDir
)

Target "BuildNuGetFlavours" (fun _ ->
    flavours
      |> Seq.iter (fun (flavour,flavourVersion) ->
            CleanDirs [nugetDir; nugetLibDir; nugetDocsDir]
        
            XCopy docsDir nugetDocsDir
            [buildDir + sprintf "Machine.Fakes.Adapters.%s.dll" flavour]
              |> CopyTo nugetLibDir

            NuGet (fun p -> 
                {p with               
                    Authors = if flavour = "NSubstitute" then "Steffen Forkmann" :: authors else authors
                    Project = sprintf "%s.%s" projectName flavour
                    Description = sprintf " This is the adapter for %s %s" flavour flavourVersion
                    Version = version                        
                    OutputPath = nugetDir
                    Dependencies = 
                        ["Machine.Fakes",version
                         flavour,flavourVersion]
                    AccessKey = getBuildParamOrDefault "nugetkey" ""
                    Publish = hasBuildParam "nugetkey" })  
                "machine.fakes.nuspec"

            [nugetDir + sprintf "Machine.Fakes.%s.%s.nupkg" flavour version]
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