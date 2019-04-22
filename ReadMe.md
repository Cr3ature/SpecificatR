# SpecificatR
[![Build Status](https://dev.azure.com/DavidVanderheyden/SpecificatR/_apis/build/status/Build.Pipeline?branchName=master)](https://dev.azure.com/DavidVanderheyden/SpecificatR/_build/latest?definitionId=7&branchName=master)
[![Build status](https://ci.appveyor.com/api/projects/status/1fx3shp4cv82qbj9?svg=true)](https://ci.appveyor.com/project/Cr3ature/specificatr)

SpecificatR contains generic repositories and interfaces for building a infrastructure. Based on EntityFramework Core and using the specification Pattern.

Supports .NET Core, .NET Standard, .NET Framework

## Get it on Nuget

The main package:
```
PM> Install-Package SpecificatR
```

The abstractions containing basemodel and specification interfaces:
```
PM> Install-Package SpecificatR.Abstractions
```

## Usage
### Registering dependencies
Registering the dependencies in an ASP.NET Core application, using Microsoft.Extensions.DependencyInjection, is pretty simple:

- Install the SpecificatR package
- Call ````services.AddSpecificatR```` inside the ````Configure```` method in Startup.cs

Need support for a different container? Feel free to [open a new issue](https://github.com/Cr3ature/SpecificatR/issues/new)

### Using SpecificatR


### Todo

- Projections: An expression to project the query to a new object. In the current version of EntityFrameworkCore, this gives issues when projecting nested entities, causing N+1 queries. [This issue should be resolved in EFCore 3](https://github.com/aspnet/EntityFrameworkCore/issues/12098#issuecomment-455997159), therefore I put this on todo list.
