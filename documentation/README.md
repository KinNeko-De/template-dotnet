# Further documentation

## Updating installed packages
`dotnet new --update-check`

Now follow the instruction

## Installing a specific version
`dotnet new --install kinneko-de.template.dotnet::0.1.60`

# Open Question:
1. Question: I got an warning.. but only one time. Maybe because of the " around github

`dotnet new --install kinneko-de.template.dotnet --nuget-source="github"`

`Die folgenden Vorlagenpakete werden installiert:`\
`   kinneko-de.template.dotnet`\
`kinneko-de.template.dotnet ist bereits installiert, Version 0.1.15 wird durch Version  ersetzt.`\
`"kinneko-de.template.dotnet::0.1.15" wurde erfolgreich deinstalliert.`\
`Warnung: Fehler beim Laden der NuGet-Quelle github: die Quelle ist ungültig. Sie wird bei der weiteren Verarbeitung übersprungen.`\
`Erfolg: kinneko-de.template.dotnet::0.1.62 installierte die folgenden Vorlagen:`\
`Vorlagenname                        Kurzname      Sprache  Tags`\
`----------------------------------  ------------  -------  ----`\
`Template for a dotnet microservice  microservice  [C#]     Web`

Answer: --nuget-source parameter is invalid here


2. Question: My access suddenly disappear and i pushed a new version of the package while my cmd was open

Answer: Not yet. I reinstalled the source. Or just create a new cmd.
