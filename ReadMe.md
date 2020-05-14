# SpecificatR
[![Build Status](https://dev.azure.com/DavidVanderheyden/SpecificatR/_apis/build/status/Build.Pipeline?branchName=master)](https://dev.azure.com/DavidVanderheyden/SpecificatR/_build/latest?definitionId=7&branchName=master)

SpecificatR contains generic repositories and interfaces for building a infrastructure. Based on EntityFramework Core and using the specification Pattern.

Supports .NET Core, .NET Standard, .NET Framework

## Get it on Nuget

The main package on [nuget.org](https://www.nuget.org/packages/SpecificatR/):
``` csharp
PM> Install-Package SpecificatR
```

The abstractions containing basemodel and specification interfaces on [nuget.org](https://www.nuget.org/packages/SpecificatR.Abstractions/):
``` csharp
PM> Install-Package SpecificatR.Abstractions
```

Abstractions for unit testing build specifications on [nuget.org](https://www.nuget.org/packages/SpecificatR.UnitTest.Abstractions/):
````csharp 
PM> Install-Package SpecificatR.UnitTest.Abstractions
````
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
            // Use Includes
            this.AddInclude(example => example.Child);

            // Use ordering asc or desc
            this.AddOrderBy(example => example.Id, OrderByDirection.Ascending);
            this.AddOrderBy(example => example.Id, OrderByDirection.Descending);

            // Use Paging
            this.ApplyPaging(pageIndex: 1, pageSize: 20);
            
            // Ignore query filters
            this.AddIgnoreQueryFilters();
            
            // Enable EF Core tracking (Default: AsNoTracking())
            this.ApplyTracking();
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

### Using the repositories
Using ReadRepository and inject in any domain or business layer class.
````csharp
// IReadRepository<TEntity, TIdentifier, TDbContext>
private readonly IReadRepository<TEntity: Example, TIdentifier: int, TDbContext: ExampleContext> _exampleRepository;

public ExampleClass(IReadRepository<TEntity: Example, TIdentifier: int, TDbContext: ExampleContext> exampleRepository)
{
    _exampleRepository = exampleRepository;
}
````

Available repository methods

##### IReadRepository
````csharp

// Get all entities with optional tracked by EF Core. Default is set to AsNoTracking
Task<TEntity[]> GetAllAsync(bool asTracking = false);

// Get all entities based on specification (Query object).
Task<TEntity[]> GetAllAsync(ISpecification<TEntity> specification);

// Get entity with optional tracked by EF Core by Id. Default is set to AsNoTracking().
Task<TEntity> GetByIdAsync(TIdentifier id, bool asTracking = false);

// Get entity based on specification (Query object).
Task<TEntity> GetSingleWithSpecificationAsync(ISpecification<TEntity> specification);
````

##### IReadWriteRepository (All methods from IReadRepository with additional write methods below)

````csharp

// Add entity to database
Task<TEntity> AddAsync(TEntity entity);

// Delete entity on database
Task DeleteByIdAsync(TIdentifier id);

// Update all properties of a entity in database
Task UpdateAsync(TEntity entity);

// Update specific properties of a entity in database
Task UpdateFieldsAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
````
