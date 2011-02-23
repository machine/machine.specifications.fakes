#I "tools/FAKE"
#r "FakeLib.dll"
open Fake
open Fake.Git

(* properties *)
let authors = ["Björn Rochel"]
let projectName = "Machine.Fakes"
let projectDescription = "Generic faking capabilites on top of Machine.Specifications"
let copyright = "Copyright - Machine.Fakes 2011"
let version = if isLocalBuild then getLastTag() else buildVersion
let title = if isLocalBuild then sprintf "%s (%s)" projectName <| getCurrentHash() else projectName

(* flavours *)
let flavours = ["RhinoMocks"; "FakeItEasy"; "Moq"]

(* Directories *)
let buildDir = @".\Build\"
let docsDir = buildDir + @"Documentation\"
let testOutputDir = buildDir + @"Specs\"
let nugetDir = buildDir + @"NuGet\" 
let testDir = buildDir
let deployDir = @".\Release\"
let targetPlatformDir = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319"

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

Target "MergeAssemblies" (fun _ ->
    flavours
      |> Seq.iter (fun flavour -> 
            let adapter = buildDir + sprintf "Machine.Fakes.Adapters.%s.dll" flavour
            let libs =
                [adapter
                 buildDir + "StructureMap.dll"
                 buildDir + "StructureMap.AutoMocking.dll"]

            ILMerge 
                (fun p -> 
                    {p with 
                        Libraries = libs
                        AttributeFile = adapter
                        Internalize = InternalizeExcept "ILMergeExcludes.txt"
                        TargetPlatform = sprintf @"v4,%s" targetPlatformDir})

                (buildDir + sprintf "Machine.Fakes.%s.dll" flavour)
                (buildDir + "Machine.Fakes.dll"))
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
        |> Scan
        |> Zip buildDir (deployDir + sprintf "%s-%s.zip" projectName version)
)

Target "BuildNuGet" (fun _ ->
    flavours
      |> Seq.iter (fun flavour ->             
            let nugetDocsDir = nugetDir + @"docs\"
            let nugetLibDir = nugetDir + @"lib\"
            CleanDirs [nugetDir; nugetLibDir; nugetDocsDir]
        
            XCopy docsDir nugetDocsDir
            [buildDir + sprintf "Machine.Fakes.%s.dll" flavour]
              |> CopyTo nugetLibDir

            NuGet (fun p -> 
                {p with               
                    Authors = authors
                    Project = sprintf "%s.%s" projectName flavour
                    Description = projectDescription       
                    Version = version                        
                    OutputPath = nugetDir
                    AccessKey = getBuildParamOrDefault "nugetkey" ""
                    Publish = hasBuildParam "nugetkey" })  
                "machine.fakes.nuspec"

            XCopy (nugetDir + sprintf "Machine.Fakes.%s.%s.nupkg" flavour version) deployDir)
)

Target "Default" DoNothing
Target "Deploy" DoNothing

// Dependencies
"BuildApp" <== ["Clean"]
"Test" <== ["BuildApp"]
"MergeAssemblies"  <== ["Test"]
"BuildZip" <== ["MergeAssemblies"]
"ZipDocumentation" <== ["GenerateDocumentation"]
"Deploy" <== ["BuildZip"; "ZipDocumentation"; "BuildNuGet"]
"Default" <== ["Deploy"]

// start build
Run <| getBuildParamOrDefault "target" "Default"