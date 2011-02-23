. .\psake_ext.ps1

$framework = '4.0'

properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\Lib"
  $build_dir = "$base_dir\Build" 
  $buildartifacts_dir = "$build_dir\" 
  $sln_file = "$base_dir\Source\Machine.Fakes.sln" 
  $tools_dir = "$base_dir\Tools"
  $release_dir = "$base_dir\Release"
  $documentation_base = "$base_dir\Documentation"
  $specs_dir = "$documentation_base\Specs"
  $docu_dir = "$documentation_base\Docu"
  $fake_framework = "All"
} 

task default -depends Docu, Specs 

# Removes the build and the release directory
task Clean { 
  Info "Removing $build_dir"
  remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue 
  
  Info "Removing $release_dir"
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
   
  Info "Removing $documentation_base"
  remove-item -force -recurse $documentation_base -ErrorAction SilentlyContinue
} 

# Initialization of the build 
#  - creation of the relates and the build dir.
#  - Generation of the GlobalAssemblyInfo.cs file. 
#      - This will use the latest Git Tag found in the current Git branch as the version number.
#      - Besides it will add the commit hash of the latest commit to the product.
task Init -depends Clean { 
    Info "Creating $release_dir"
    new-item $release_dir -itemType directory 
    
    Info "Creating $build_dir"
    new-item $build_dir -itemType directory 
    
    Info "Generating GlobalAssemblyInfo.cs @ '$base_dir\Source\GlobalAssemblyInfo.cs'"
    Generate-Assembly-Info `
        -file "$base_dir\Source\GlobalAssemblyInfo.cs" `
        -title "Machine.Fakes" `
        -description "Generic faking capabilites on top of Machine.Specifications" `
        -product "Machine.Fakes" `
        -clsCompliant "false" `
        -copyright "Copyright © Machine.Fakes 2011"
} 

# Compiles the solution to the global build directory
task Compile -depends Init { 
  exec { msbuild /t:rebuild "/p:OutDir=$buildartifacts_dir" "/p:Configuration=Release" "/p:Platform=Any CPU" "$sln_file" }
} 

# Creates the documentation
task Docu -depends Compile {
  Info "Creating $docu_dir"
  new-item $docu_dir -itemType directory 

  exec { & $tools_dir\docu\docu.exe $build_dir\Machine.Fakes.dll --output=$docu_dir }
}

# Runs mspec against the machine.fakes specifications
task Specs -depends Compile {
  Info "Creating $specs_dir"
  new-item $specs_dir -itemType directory 
  
  exec { & $tools_dir\MSpec\mspec.exe --html $specs_dir $build_dir\Machine.Fakes.Specs.dll $build_dir\Machine.Fakes.Adapters.Specs.dll }
}

# Merges the assemblies together into a bundle that is configured for rhino.mocks usage
task Merge_Rhino -depends Compile {
  
  $framework_dir = Get-FrameworkDirectory
  $old = pwd
  cd $build_dir
    
  Remove-Item Machine.Fakes.RhinoMocks.dll -ErrorAction SilentlyContinue 
 
  exec {
        
     & $tools_dir\ILMerge\ILMerge.exe /log:ilmerge.txt /out:Machine.Fakes.RhinoMocks.dll /attr:Machine.Fakes.Adapters.Rhinomocks.dll `
        Machine.Fakes.dll `
        Machine.Fakes.Adapters.Rhinomocks.dll `
        StructureMap.dll `
        StructureMap.AutoMocking.dll `
        "/internalize:$base_dir\ILMergeExcludes.txt" `
        /t:library  `
        /targetplatform:"v4,$framework_dir"
  }
     
  cd $old
}