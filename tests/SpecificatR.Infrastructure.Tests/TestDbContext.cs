using SpecificatR.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;

namespace SpecificatR.Infrastructure.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TestEntity> TestEntities { get; set; }
    }

    public class TestEntity : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }
    }
}
