//-----------------------------------------------------------------------
// <copyright file="SpecificationTests.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 17:47:14</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Tests
{
    using AutoFixture;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Tests.Specifications;
    using SpecificatR.Infrastructure.UnitTest.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="SpecificationTests"/>
    /// </summary>
    public class SpecificationTests
    {
        /// <summary>
        /// Defines the _fixture
        /// </summary>
        private readonly IFixture _fixture = new Fixture();

        /// <summary>
        /// Defines the _options
        /// </summary>
        private readonly DbContextOptions<TestDbContext> _options = new DbContextOptions<TestDbContext>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationTests"/> class.
        /// </summary>
        public SpecificationTests()
        {
            _fixture.Customize<TestEntity>(te => te.Without(w => w.Children));
        }

        /// <summary>
        /// The Should_ApplyPaging
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task Should_ApplyPaging()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(4).ToArray();

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] result = await mockUnitTestSpecification.GetAllAsync(entities.AsQueryable(), new TestEntityPaginatedSpecification(1, 2));

            // Assert
            result.Should().HaveCount(2);
            result[0].Id.Should().Equals(entities[0]);
            result[1].Id.Should().Equals(entities[1]);
        }

        [Fact]
        public async Task Should_OrderByNameAscending()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(4).ToArray();

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] result = await mockUnitTestSpecification.GetAllAsync(entities.AsQueryable(), new TestEntityOrderByNameAscSpecification());
            TestEntity[] orderedList = entities.OrderBy(o => o.Name).ToArray();

            // Assert
            result.Should().Equals(orderedList);
        }

        [Fact]
        public async Task Should_OrderByNameDescending()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(4).ToArray();

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] result = await mockUnitTestSpecification.GetAllAsync(entities.AsQueryable(), new TestEntityOrderByNameDescSpecification());
            TestEntity[] orderedList = entities.OrderByDescending(o => o.Name).ToArray();

            // Assert
            result.Should().Equals(orderedList);
        }

        /// <summary>
        /// The Should_SetCriteria
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task Should_SetCriteria()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] result = await mockUnitTestSpecification.GetAllAsync(entities.AsQueryable(), new TestEntityByIdSpecification(entities[0].Id));

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(entities[0]);
        }

        /// <summary>
        /// The CharactersOfTypeHumanSpecification_ShouldApplySpecification
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task TestEntityWithChildEntitiesSpecification_ShouldApplySpecification()
        {
            // Arrange
            var parentGuid = Guid.NewGuid();
            var childEntity = _fixture.Build<TestEntityChild>()
                .Without(wh => wh.Parent)
                .With(wh => wh.ParentId, parentGuid)
                .Create();
            var testEntitiesWithChild = _fixture.Build<TestEntity>()
                .With(w => w.Id, parentGuid)
                .With(w => w.Children, new List<TestEntityChild> { childEntity })
                .CreateMany(3);

            var testEntitiesWithoutChildren = _fixture.Build<TestEntity>()
                .Without(wh => wh.Children)
                .CreateMany(5);

            List<TestEntity> testEntities = new List<TestEntity>();
            testEntities.AddRange(testEntitiesWithoutChildren);
            testEntities.AddRange(testEntitiesWithChild);

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] testEntityResults = await mockUnitTestSpecification.GetAllAsync(testEntities.AsQueryable(), new TestEntityWithChildEntitiesSpecification());

            // Assert
            testEntityResults.Should().NotBeNull().And.HaveSameCount(testEntitiesWithChild);
        }
    }
}
