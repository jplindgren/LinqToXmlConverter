@echo off
cls

ls

echo "Installing Fake..."
"tools\Nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"

echo "Fake Installed!!"
ls
"packages\FAKE\tools\Fake.exe" build.fsx
rem pause
