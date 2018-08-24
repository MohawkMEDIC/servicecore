	@ECHO off

cmd /c del release\*.nupkg

if exist "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" (
	echo will use VS 2017 build tools
	"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" ..\ServiceCore.sln /t:clean /t:rebuild /p:configuration=Debug /m
) else ( 
	if exist "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" (
		echo will use VS 2017 build tools
		"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" ..\ServiceCore.sln /t:clean /t:rebuild /p:configuration=Debug /m
	)
	else (
		echo Unable to locate VS 2017 build tools, will use default build tools 
	)
)

for /D %%D in (..\MARC.HI.EHRS.*) do (	
	echo found directory %%D
	
	echo will create nupkg file
	nuget pack %%D -IncludeReferencedProjects -OutputDirectory %localappdata%\NugetStaging -prop Configuration=Debug -Symbols
)
