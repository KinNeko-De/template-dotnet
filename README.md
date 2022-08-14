**Automation is necessary for lazy people**

# Template
Project for all templates

# Install templates
On windows run ``` build-and-install-templates.cmd ``` to install all templates.

# New project
The template must be used to create new projects

## Microservice
To create a new project based on the ```microservice``` template use  

```bat
dotnet new microservice --name-dotnet <MyNameOfDomain> --name-project <MyNameOfProject>
``` 

*Example*
```bat
dotnet new microservice --name-dotnet My-Domain --name-project My-Project
``` 

### Database
If the microservice use a database, you must use flyway to patch the database. Use the ```--enable-flyway``` flag.

```bat
dotnet new microservice --name-dotnet <MyNameOfSolution> --name-project <MyNameOfProject> --enable-flyway
```

# Replacing variables

***From dotnet templating***
Choose the right output depending of your context

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
Identity             | Template.1 | My-App | Template.1 -> My-App
Namespace            | Template.1 | My-App | Template._1 -> My_App
Class Name           | Template.1 | My-App | Template__1 -> My_App 
Lowercase Identity   | Template.1 | My-App | template.1 -> my-app
Lowercase Namespace  | Template.1 | My-App | template._1 -> my_app
Lowercase Class Name | Template.1 | My-App | template__1 -> my_app


# Sources
[Dotnet Templating](https://github.com/dotnet/templating/wiki)
