@ECHO off

for /R %%f in (*.nupkg) do (	
	echo found nupkg file %%f
	
	echo will deploy nupkg file
	
	rem nuget push %%f SERVICE_CORE_API_KEY_HERE -Source https://api.nuget.org/v3/index.json
)
