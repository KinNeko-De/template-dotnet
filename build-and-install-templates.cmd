del kinneko-de.template.dotnet.0.1.0-local.nupkg
dotnet new uninstall kinneko-de.template.dotnet
dotnet pack .\KinNeko-De.Template.Dotnet.csproj -o . --version-suffix local
dotnet new install .\kinneko-de.template.dotnet.0.1.0-local.nupkg
pause