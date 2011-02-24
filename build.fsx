#I "tools/FAKE"
#r "FakeLib.dll"
open Fake
open Fake.Git

(* properties *)
let authors = ["Bjoern Rochel"]
let projectName = "Machine.Fakes"
let projectDescription = "Generic faking capabilites on top of Machine.Specifications"
let copyright = "Copyright - Machine.Fakes 2011"
let versionFile = "version.txt"
let version = 
    if not isLocalBuild then ReadFile versionFile |> Seq.head else
    let tag = getLastTag()
    ReplaceFile versionFile tag
    tag

let title = if isLocalBuild then sprintf "%s (%s)" projectName <| getCurrentHash() else projectName

(* flavours *)
let flavours = 
    ["RhinoMocks","3.6"; 
     "FakeItEasy","1.6.4062.205";
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
            AssemblyDescription = projectDescription;
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
            Description = projectDescription       
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
                    Authors = authors
                    Project = sprintf "%s.%s" projectName flavour
                    Description = sprintf "%s - Bundeled with %s %s" projectDescription flavour flavourVersion
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

// Dependencies
"BuildApp" <== ["Clean"]
"Test" <== ["BuildApp"]
"MergeStructureMap" <== ["Test"]
"BuildZip" <== ["MergeStructureMap"]
"ZipDocumentation" <== ["GenerateDocumentation"]
"BuildNuGetFlavours" <== ["BuildNuGet"]
"Deploy" <== ["BuildZip"; "ZipDocumentation"; "BuildNuGetFlavours"]
"Default" <== ["Deploy"]

// start build
Run <| getBuildParamOrDefault "target" "Default"