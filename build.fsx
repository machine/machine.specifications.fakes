#r @"Source\packages\FAKE\tools\FakeLib.dll"
#r "System.Web.Extensions.dll"

open Fake
open Fake.Git
open Fake.AssemblyInfoFile
open System.IO
open System.Net

(* properties *)
let authors = ["The machine project"]
let projectName = "Machine.Fakes"
let copyright = "Copyright - Machine.Fakes 2011 - 2013"
// none as workaround for a FAKE bug
let NugetKey = if System.IO.File.Exists @".\key.txt" then ReadFileAsString @".\key.txt" else "none"

let ExecuteGetCommand (url:string) =
    use client = new WebClient()
    client.Headers.Add("User-Agent", "Machine.Fakes/teamcity.codebetter.com")
  
    use stream = client.OpenRead(url)
    use reader = new StreamReader(stream)
    reader.ReadToEnd()

let release =
    ReadFile "changelog.markdown"
    |> ReleaseNotesHelper.parseReleaseNotes

let title = if isLocalBuild then sprintf "%s (%s)" projectName <| getCurrentHash() else projectName


(* Directories *)
let buildDir = @".\Build\"
let packagesDir = @".\Source\packages\"
let testOutputDir = buildDir + @"Specs\"
let nugetDir = buildDir + @"NuGet\"
let deployDir = @".\Release\"
let nugetLibDir = nugetDir + @"lib\net40\"

(* files *)
let slnReferences = !! @".\Source\*.sln"

(* flavours *)
let Flavours = ["RhinoMocks"; "FakeItEasy"; "NSubstitute"; "Moq"]
let MSpecVersion() = GetPackageVersion packagesDir "Machine.Specifications"
let mspecTool() = sprintf @".\Source\packages\Machine.Specifications.%s\tools\mspec-clr4.exe" (MSpecVersion())

(* Targets *)
Target "Clean" (fun _ -> CleanDirs [nugetDir; buildDir; deployDir; testOutputDir] )

Target "BuildApp" (fun _ ->
    CreateCSharpAssemblyInfo @".\Source\GlobalAssemblyInfo.cs"
        [Attribute.Version release.AssemblyVersion
         Attribute.Title title
         Attribute.Product title
         Attribute.Description "An integration layer for fake frameworks on top of MSpec"
         Attribute.Copyright copyright
         Attribute.Guid "3745F3DA-6ABB-4C58-923D-B09E4A04688F"
         // specifying DelaySign explicitly because of a bug in FAKE
         Attribute.BoolAttribute("AssemblyDelaySign", false, "System.Reflection")
         Attribute.FileVersion release.AssemblyVersion
         Attribute.ComVisible false
         Attribute.CLSCompliant false]

    slnReferences
        |> MSBuildRelease buildDir "Build"
        |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
    ActivateFinalTarget "DeployTestResults"
    !! (buildDir + "/*.Specs.dll")
      ++ (buildDir + "/*.Examples.dll")
        |> MSpec (fun p ->
                    {p with
                        ToolPath = mspecTool()
                        HtmlOutputDir = testOutputDir})
)

FinalTarget "DeployTestResults" (fun () ->
    !! (testOutputDir + "\**\*.*")
      |> Zip testOutputDir (sprintf "%sMSpecResults.zip" deployDir)
)

Target "BuildZip" (fun _ ->
    !! (buildDir + "/**/*.*")
      -- "*.zip"
      -- "**/*.Specs.*"
        |> Zip buildDir (deployDir + sprintf "%s-%s.zip" projectName release.AssemblyVersion)
)

let RequireAtLeast version = sprintf "%s" <| NormalizeVersion version

Target "BuildNuGet" (fun _ ->
    CleanDirs [nugetDir; nugetLibDir]

    [buildDir + "Machine.Fakes.dll"; buildDir + "Machine.Fakes.xml"]
        |> CopyTo nugetLibDir
    
    NuGet (fun p ->
        {p with
            Summary = "A framework for faking dependencies on top of Machine.Specifications."
            Authors = authors
            Project = projectName
            Version = release.AssemblyVersion
            OutputPath = deployDir
            Dependencies = ["Machine.Specifications",RequireAtLeast (MSpecVersion())]
            AccessKey = NugetKey
            Publish = NugetKey <> "none"
            ToolPath = @".\Source\.nuget\nuget.exe"
            WorkingDir = nugetDir })
        "machine.fakes.nuspec"
)

Target "BuildNuGetFlavours" (fun _ ->
    Flavours
      |> Seq.iter (fun (flavour) ->
            let flavourVersion = GetPackageVersion packagesDir flavour
            CleanDirs [nugetDir; nugetLibDir]

            [buildDir + sprintf "Machine.Fakes.Adapters.%s.dll" flavour; buildDir + sprintf "Machine.Fakes.Adapters.%s.xml" flavour]
              |> CopyTo nugetLibDir

            NuGet (fun p ->
                {p with
                    Summary = sprintf "A framework for faking objects with %s on top of Machine.Specifications." flavour
                    Authors = authors
                    Project = sprintf "%s.%s" projectName flavour
                    Description = sprintf " This is the adapter for %s %s" flavour flavourVersion
                    Version = release.AssemblyVersion
                    OutputPath = deployDir
                    Dependencies =
                        ["Machine.Fakes",RequireExactly (NormalizeVersion release.AssemblyVersion)
                         flavour,RequireAtLeast flavourVersion]
                    AccessKey = NugetKey
                    Publish = NugetKey <> "none"
                    ToolPath = @".\Source\.nuget\nuget.exe"
                    WorkingDir = nugetDir })
                "machine.fakes.nuspec"
    )
)

Target "Default" DoNothing
Target "Deploy" DoNothing

// Build order
"Clean"
  ==> "BuildApp"
  ==> "Test"
  ==> "BuildZip"
  ==> "BuildNuGet"
  ==> "BuildNuGetFlavours"
  ==> "Deploy"
  ==> "Default"

// start build
RunParameterTargetOrDefault  "target" "Default"
