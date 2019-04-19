# SpecificatR

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
