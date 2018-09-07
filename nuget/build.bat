@ECHO off

cmd /c del release\*.nupkg


if exist "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" (
        echo will use VS 2017 build tools
        set msbuild="C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"
) else (
	if exist "c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" (
        echo will use VS 2017 build tools
        set msbuild="c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
	) else ( echo Unable to locate VS 2017 build tools, will use default build tools )
)

if defined msbuild (

%msbuild% ..\ServiceCore.sln /t:clean /m
%msbuild% ..\ServiceCore.sln /t:rebuild /p:configuration=release /m

for /D %%D in (..\MARC.HI.EHRS.*) do (	
	echo found directory %%D
	
	echo will create nupkg file
	nuget pack %%D -IncludeReferencedProjects -OutputDirectory %localappdata%\NugetStaging -prop Configuration=Release
)
)