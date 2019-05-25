//-----------------------------------------------------------------------
// <copyright file="TestSpecifications.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 17:46:12</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Tests.Specifications
{
    using SpecificatR.Infrastructure.Abstractions;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="TestEntityByIdSpecification"/>
    /// </summary>
    public class TestEntityByIdSpecification : BaseSpecification<TestEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntityByIdSpecification"/> class.
        /// </summary>
        /// <param name="id">The id <see cref="Guid"/></param>
        public TestEntityByIdSpecification(Guid id)
            : base(BuildCriteria(id))
        {
        }

        /// <summary>
        /// The BuildCriteria
        /// </summary>
        /// <param name="id">The id <see cref="Guid"/></param>
        /// <returns>The <see cref="Expression{Func{TestEntity, bool}}"/></returns>
        private static Expression<Func<TestEntity, bool>> BuildCriteria(Guid id)
            => (x => x.Id == id);
    }

    /// <summary>
    /// Defines the <see cref="TestEntityOrderByNameAscSpecification"/>
    /// </summary>
    public class TestEntityOrderByNameAscSpecification : BaseSpecification<TestEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntityOrderByNameAscSpecification"/> class.
        /// </summary>
        public TestEntityOrderByNameAscSpecification()
            : base(null)
        {
            AddOrderBy(o => o.Name, OrderByDirection.Ascending);
        }
    }

    /// <summary>
    /// Defines the <see cref="TestEntityOrderByNameDescSpecification"/>
    /// </summary>
    public class TestEntityOrderByNameDescSpecification : BaseSpecification<TestEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntityOrderByNameDescSpecification"/> class.
        /// </summary>
        public TestEntityOrderByNameDescSpecification()
            : base(null)
        {
            AddOrderBy(o => o.Name, OrderByDirection.Descending);
        }
    }

    /// <summary>
    /// Defines the <see cref="TestEntityPaginatedSpecification"/>
    /// </summary>
    public class TestEntityPaginatedSpecification : BaseSpecification<TestEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntityPaginatedSpecification"/> class.
        /// </summary>
        /// <param name="pageIndex">The pageIndex <see cref="int"/></param>
        /// <param name="pageSize"> The pageSize <see cref="int"/></param>
        public TestEntityPaginatedSpecification(int pageIndex, int pageSize)
            : base(null)
        {
            ApplyPaging(pageIndex, pageSize);
        }
    }

    /// <summary>
    /// Defines the <see cref="TestEntityWithChildEntitiesSpecification"/>
    /// </summary>
    public class TestEntityWithChildEntitiesSpecification : BaseSpecification<TestEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntityWithChildEntitiesSpecification"/> class.
        /// </summary>
        public TestEntityWithChildEntitiesSpecification()
            : base(BuildCriteria())
        {
        }

        /// <summary>
        /// The BuildCriteria
        /// </summary>
        /// <returns>The <see cref="Expression{Func{TestEntity, bool}}"/></returns>
        private static Expression<Func<TestEntity, bool>> BuildCriteria()
        {
            return x => x.Children != null && x.Children.Any();
        }
    }
}
