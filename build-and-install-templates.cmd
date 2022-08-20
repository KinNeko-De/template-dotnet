del kinneko-de.template.dotnet.0.1.0-local.nupkg
dotnet new --uninstall kinneko-de.template.dotnet
dotnet pack .\templatepack.csproj -o . --version-suffix local
dotnet new -i .\kinneko-de.template.dotnet.0.1.0-local.nupkg
pause