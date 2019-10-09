//-----------------------------------------------------------------------
// <copyright file="TestDbContext.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 11:04:30</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;

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
