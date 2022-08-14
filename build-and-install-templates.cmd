del kinneko-de.template.dotnet.1.0.0.nupkg
dotnet new --uninstall kinneko-de.template.dotnet
dotnet pack .\templatepack.csproj -o .
dotnet new -i .\kinneko-de.template.dotnet.1.0.0.nupkg
pause