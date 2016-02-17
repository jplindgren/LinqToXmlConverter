@echo off
cls
"tools\Nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"

"packages\FAKE\tools\Fake.exe" build.fsx
pause