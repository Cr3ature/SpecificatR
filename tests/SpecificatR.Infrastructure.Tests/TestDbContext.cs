//-----------------------------------------------------------------------
// <copyright file="TestDbContext.cs" >
//     Copyright (c) 2019-2020 David Vanderheyden All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Abstractions;

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TestEntity> TestEntities { get; set; }

        public virtual DbSet<TestEntityChild> TestEntityChildren { get; set; }
    }

    public class TestEntity : IBaseEntity<Guid>
    {
        public virtual ICollection<TestEntityChild> Children { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }
    }

    public class TestEntityChild : IBaseEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TestEntity Parent { get; set; }

        public Guid ParentId { get; set; }
    }
}
