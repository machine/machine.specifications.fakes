FormatTaskName {
   param($taskName)
   write-host ("-"*25) "Executing Task: $taskName" ("-"*25) -foregroundcolor Yellow
}

function Info {
   param([string]$message)
   Write-Host $message -ForegroundColor Green
}

function Get-Git-Tag {
    git describe
}

function Get-Git-Commit {
	$gitLog = git log --oneline -1
	$gitLog = $gitLog.Split(' ') 
	$tmpString = $gitLog[0].Split('m') 
	$index = 0
	if ($tmpString.Length -gt 2)
	{
		$index=1
	} 	
	return $tmpString[$index].SubString(0,6)
}

function Generate-Assembly-Info {
   param(
	[string]$clsCompliant = "true",
	[string]$title, 
	[string]$description, 
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$file = $(throw "file is a required parameter."))
  
  $version = Get-Git-Tag
  $commit = Get-Git-Commit
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: CLSCompliantAttribute($clsCompliant )]
[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyDescriptionAttribute(""$description"")]
[assembly: AssemblyProductAttribute(""$product ($commit)"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyInformationalVersionAttribute(""$version"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyDelaySignAttribute(false)]

"
	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $file"
	Write-Output $asmInfo > $file
}

function Get-FrameworkDirectory() { 
    $([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory().Replace("v2.0.50727", "v4.0.30319")) 
}