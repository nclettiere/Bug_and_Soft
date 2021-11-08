Install-Module powershell-yaml

$version=$args[0]

$installerPath = -join([Environment]::GetFolderPath("MyDocuments"), "\PacoBuilds\Instaladores\PacoSetup-Win64-", $version, ".exe");
$installerDwnl = -join('https://bugnsoft.github.io/PacoSetup-Win64-', $version, ".exe");


$repoPath = -join([Environment]::GetFolderPath("MyDocuments"), "\Projects\bugnsoft.github.io");
$configFile = -join([Environment]::GetFolderPath("MyDocuments"), "\Projects\bugnsoft.github.io\", "_config.yml");

Set-Location -Path $repoPath -PassThru

Start-Process -FilePath "C:\Program Files\Git\bin\git.exe" -ArgumentList "pull"
Copy-Item $installerPath -Destination $repoPath

$fileContent = Get-Content -Path $configFile
$content = ''
foreach ($line in $fileContent) { $content = $content + "`n" + $line }
$yaml = ConvertFrom-YAML $content

Write-Host $yaml

#$config_obj.github.instalador = $installerDwnl;
#foreach($line in [System.IO.File]::ReadLines("C:\path\to\file.txt"))
#{
#       $line
#}