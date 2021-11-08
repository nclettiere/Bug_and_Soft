<#
    .Description
    Este programa asume que la instalacion de Unity se encuentra en
	C:\Program Files\Unity\Hub\Editor\2020.3.12f1\Editor\Unity.exe
	Y a su vez se asume que este script es llamado desde la carpeta del proyecto de Unity
	
	!!!> Una vez compilado el juego, usamos el programa InnoSetup para crear el hermoso instalador paquense
#>

$version=$args[0]

$pacoBuildOutput = -join([Environment]::GetFolderPath("MyDocuments"), "\PacoBuilds\Win64");
$logfile = -join([Environment]::GetFolderPath("MyDocuments"), "\PacoBuilds\log.txt");
$unityArguments = -join('-quit -batchmode -logFile ', $logfile, ' -projectPath C:\Users\Nicolini\Documents\Projects\Bug_and_Soft -executeMethod Builder.build');
$innoArguments = -join('/DMyAppVersion=', $version, " /DPacoFiles=", $pacoBuildOutput, " paco_installer_inno_compiler.iss");

Write-Host "==========================================================="
Write-Host "Action ejectutado para la version: $version"
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
Write-Host "Subir Artifacts a Github Actions"
Write-Host "==========================================================="

exit $inno.ExitCode