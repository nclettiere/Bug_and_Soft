Install-Module powershell-yaml

$version=$args[0]
$installerPath = -join('C:\Users\Nicolini\Documents\PacoBuilds\Instaladores\PacoSetup-Win64-', $version, ".exe");
$installerDwnl = -join('https://bugnsoft.github.io/PacoSetup-Win64-', $version, ".exe");

Set-Location -Path "C:\Users\Nicolini\Documents\Projects\bugnsoft.github.io" -PassThru

Start-Process -FilePath "C:\Program Files\Git\bin\git.exe" -ArgumentList "pull"
Copy-Item $installerPath -Destination "C:\Users\Nicolini\Documents\Projects\bugnsoft.github.io"

$fileContent = Get-Content -Path "C:\Users\Nicolini\Documents\Projects\bugnsoft.github.io\_config.yml"
$content = ''
foreach ($line in $fileContent) { $content = $content + "`n" + $line }
$yaml = ConvertFrom-YAML $content

Write-Host $yaml

#$config_obj.github.instalador = $installerDwnl;
#foreach($line in [System.IO.File]::ReadLines("C:\path\to\file.txt"))
#{
#       $line
#}


Set-Location -Path "C:\Users\Nicolini\Documents\Projects\Bug_and_Soft" -PassThru