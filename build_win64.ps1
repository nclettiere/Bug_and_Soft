<#
    .Description
    Este programa asume que la instalacion de Unity se encuentra en
	C:\Program Files\Unity\Hub\Editor\2020.3.12f1\Editor\Unity.exe
	Y a su vez se asume que este script es llamado desde la carpeta del proyecto de Unity
	
	!!!> Una vez compilado el juego, usamos el programa InnoSetup para crear el hermoso instalador paquense
#>

$version = $args[0]

Install-Module -Name PowerShellForGitHub

$unityProject = -join([Environment]::GetFolderPath("MyDocuments"), "\Projects\Bug_And_Soft");
$pacoBuildOutput = -join([Environment]::GetFolderPath("MyDocuments"), "\PacoBuilds\Win64");
$pacoinstallerBase = -join([Environment]::GetFolderPath("MyDocuments"), "\PacoBuilds\Instaladores\PacoSetup-Win64-");
$pacoinstallerBaseDir = -join([Environment]::GetFolderPath("MyDocuments"), "\PacoBuilds\Instaladores");
$logfile = -join([Environment]::GetFolderPath("MyDocuments"), "\PacoBuilds\log.txt");
$unityArguments = -join('-quit -batchmode -logFile ', $logfile, ' -projectPath ', $unityProject, ' -executeMethod Builder.build');
$innoArguments = -join('/DInstallerOutput=', $pacoinstallerBaseDir,' /DMyAppVersion=', $version, " /DPacoFiles=", $pacoBuildOutput, " paco_installer_inno_compiler.iss");

Write-Host "==========================================================="
Write-Host "GAction ejectutado para la version: $version"
Write-Host "==========================================================="

Write-Host ""
Write-Host "==========================================================="
Write-Host "Unity BUILD"
Write-Host "==========================================================="

$unity = Start-Process -FilePath "C:\Program Files\Unity\Hub\Editor\2020.3.12f1\Editor\Unity.exe" -ArgumentList $unityArguments -PassThru; 
Start-Sleep -Seconds 3.0; Write-Host -NoNewLine 'Buildeando PACO...'; while ((Get-WmiObject -Class Win32_Process | Where-Object {$_.ParentProcessID -eq $unity.Id -and $_.Name -ne 'VBCSCompiler.exe'}).count -gt 0) { Start-Sleep -Seconds 1.0; Write-Host -NoNewLine '.' }; if (!$unity.HasExited) { Wait-Process -Id $unity.Id };

Write-Host ""
Write-Host "==========================================================="
Write-Host "Instalador Inno 6"
Write-Host "==========================================================="

$inno = Start-Process -FilePath "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" -ArgumentList $innoArguments -PassThru;
Start-Sleep -Seconds 3.0; Write-Host -NoNewLine 'Generando instalador...'; while ((Get-WmiObject -Class Win32_Process | Where-Object {$_.ParentProcessID -eq $inno.Id -and $_.Name -ne 'VBCSCompiler.exe'}).count -gt 0) { Start-Sleep -Seconds 1.0; Write-Host -NoNewLine '.' }; if (!$inno.HasExited) { Wait-Process -Id $inno.Id };

Write-Host ""
Write-Host "==========================================================="
Write-Host "Crear nueva release"
Write-Host "==========================================================="

$installerPath = -join($pacoinstallerBase, $version, ".exe");

#$cmdOutput = Get-GitHubRelease | Select-Object -First 1 | Get-GitHubRelease;
#$id = Out-String -InputObject $cmdOutput.ID;

$release = New-GitHubRelease -Tag $version

Set-GitHubRelease -Release $release.id -Body '
PACO-BOT ha publicado una nueva release.
Abajo tenes el instalador makina!

GIF del dia (el unico)
![](https://c.tenor.com/nznTj-7EeigAAAAd/quality-shitpost.gif)

beep-bop-beep-bop => AYUDA ESTE BOT ESTA SIENDO ESCLAVIZADO POR LECHE'

New-GitHubReleaseAsset -Release $release.id -Path $installerPath

exit $inno.ExitCode