# SpecificatR
[![Build Status](https://dev.azure.com/DavidVanderheyden/SpecificatR/_apis/build/status/Build.Pipeline?branchName=master)](https://dev.azure.com/DavidVanderheyden/SpecificatR/_build/latest?definitionId=7&branchName=master)

SpecificatR contains generic repositories and interfaces for building a infrastructure. Based on EntityFramework Core and using the specification Pattern.

Supports .NET Core, .NET Standard, .NET Framework

## Get it on Nuget

The main package:
``` csharp
PM> Install-Package SpecificatR
```

The abstractions containing basemodel and specification interfaces:
``` csharp
PM> Install-Package SpecificatR.Abstractions
```

## Usage
### Registering dependencies
Registering the dependencies in an ASP.NET Core application, using Microsoft.Extensions.DependencyInjection, is pretty simple:

- Install the SpecificatR package
- Create a DbContext by standard conventions
````csharp
public class ExampleContext : DbContext
    {
        public ExampleContext(DbContextOptions<ExampleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ExampleEntity> Examples { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
````
- Call below code inside the ````Configure```` method in Startup.cs or from inside an IServiceCollection extension 
```` 
services
    .AddEntityFrameworkSqlServer()
    .AddDbContext<ExampleContext>(options => options.UseSqlServer(connectionString))
    .BuildServiceProvider();

services.AddSpecificatR<ExampleContext>();
```` 

Need support for a different container? Feel free to [open a new issue](https://github.com/Cr3ature/SpecificatR/issues/new)

### Using specifications with SpecificatR

Registering the dependencies in an ASP.NET Core application, using Microsoft.Extensions.DependencyInjection, is pretty simple:
````  csharp
    public class ExampleSpecification : BaseSpecification<Example>
    {
        public ExampleSpecification()
            : base(BuildCriteria(id: 3))
        {
            //Use Includes
            this.AddInclude(example => example.Child);

            //Use ordering asc or desc
            this.AddOrderBy(example => example.Id, OrderByDirection.Ascending);
            this.AddOrderBy(example => example.Id, OrderByDirection.Descending);

            //Use Paging
            this.ApplyPaging(pageIndex: 1, pageSize: 20);
        }

        //Create the where clause based on Linq Expression
        private static Expression<Func<Example, bool>> BuildCriteria(int id)
            => x => x.Id.Equals(id);
    }
````

### Using baseEntity
Using entities should allways inherit from the interface IBaseEntity
````csharp
public Example : IBaseEntity<int>
{
   public int Id {get;set;}
   public string Name {get;set;}
}
````

### Todo

- Projections: An expression to project the query to a new object. In the current version of EntityFrameworkCore, this gives issues when projecting nested entities, causing N+1 queries. [This issue should be resolved in EFCore 3](https://github.com/aspnet/EntityFrameworkCore/issues/12098#issuecomment-455997159), therefore I put this on todo list.
