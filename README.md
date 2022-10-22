**Automation is necessary for lazy people**

# Template
Project for dotnet templates

# Install templates
## local version
You can build a local prelease version and install this to test it.

On windows run ``` build-and-install-templates.cmd ``` to install all templates.

## install version from nuget feed
1. Create a [personal access](https://github.com/settings/tokens) token with the right *``` read:packages ```*
2. Add the nuget source to the nuget feed.
```bat
dotnet nuget add source https://nuget.pkg.github.com/KinNeko-De/index.json --name github --username <MY_USER> --password <MY_PAT>
```
3. Install the template
```bat
dotnet new --install kinneko-de.template.dotnet
```

Read the [documentation](documentation/README.md) for more information.

# New project
The template must be used to create new projects

## Microservice
To create a new project based on the ```microservice``` template use  

```bat
dotnet new microservice --name-dotnet <MyNameOfDomain> --name-project <MyNameOfProject>
``` 

*Example*
```bat
dotnet new microservice --name-dotnet SvcExample --name-project Template --name-domain Example
``` 

### Database
The microservice is created with a database by default.

If the microservice do not need a database, you must remove the database. Use the ```--database false``` flag.

```bat
dotnet new microservice [..] --database false
```

# Replacing variables

***From dotnet templating***

Choose the right output depending of your context.

Transform | Input | Output
-------|------|--------
Identity | Template.1 | Template.1
Namespace | Template.1 | Template._1
Class Name | Template.1 | Template__1
Lowercase Identity | Template.1 | template.1
Lowercase Namespace | Template.1 | template._1
Lowercase Class Name | Template.1 | template__1


***Full mapping***
Transform | sourceName | name | generated replacement
-------|------|--------|--------
Identity             | Template.1 | My-App.1 | Template.1 -> My-App.1
Namespace            | Template.1 | My-App.1 | Template._1 -> My_App.1
Class Name           | Template.1 | My-App.1 | Template__1 -> My_App_1 
Lowercase Identity   | Template.1 | My-App.1 | template.1 -> my-app.1
Lowercase Namespace  | Template.1 | My-App.1 | template._1 -> my_app.1
Lowercase Class Name | Template.1 | My-App.1 | template__1 -> my_app_1

***Note***
Not all replacement are already implemented in the template

# Sources
[Dotnet Templating](https://github.com/dotnet/templating/wiki)

[ConditionalReplacement](https://github.com/dotnet/templating/wiki/Conditional-processing-and-comment-syntax)

[Examples](https://github.com/dotnet/dotnet-template-samples)

[SymbolGenerator](https://github.com/dotnet/templating/wiki/Available-Symbols-Generators)