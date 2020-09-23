namespace SpecificatR.Infrastructure.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using SpecificatR.Infrastructure.Tests.Specifications;
    using SpecificatR.UnitTest.Abstractions;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="SpecificationTests"/>.
    /// </summary>
    public class SpecificationTests
    {
        /// <summary>
        /// Defines the _fixture.
        /// </summary>
        private readonly IFixture _fixture = new Fixture();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationTests"/> class.
        /// </summary>
        public SpecificationTests()
        {
            _fixture.Customize<TestEntity>(te => te.Without(w => w.Children));
        }

        /// <summary>
        /// The TestEntityPaginatedWithOrderBySpecification should apply.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetAll_WithPaginatedAndOrderBySpecification_ShouldGetSecondPaginatedResult()
        {
            // Arrange
            IEnumerable<TestEntity> testEntities = _fixture.CreateMany<TestEntity>(25);

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] result = await mockUnitTestSpecification.GetAll(testEntities.AsQueryable(), new TestEntityPaginatedWithOrderbySpecification(2, 10));
            TestEntity[] wantedResult = testEntities.OrderByDescending(o => o.Name).Skip(10).Take(10).ToArray();

            // Assert
            result.Should().Equals(wantedResult);
        }

        /// <summary>
        /// The TestEntityPaginatedWithOrderBySpecification should apply.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetAll_WithPaginatedAndOrderBySpecification_ShouldGetPaginated()
        {
            // Arrange
            IEnumerable<TestEntity> testEntities = _fixture.CreateMany<TestEntity>(25);

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] result = await mockUnitTestSpecification.GetAll(testEntities.AsQueryable(), new TestEntityPaginatedWithOrderbySpecification(1, 10));
            TestEntity[] wantedResult = testEntities.OrderByDescending(o => o.Name).Take(10).ToArray();

            // Assert
            result.Should().Equals(wantedResult);
        }

        /// <summary>
        /// The Should_ApplyPaging.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_ApplyPaging()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(4).ToArray();

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] result = await mockUnitTestSpecification.GetAll(entities.AsQueryable(), new TestEntityPaginatedSpecification(1, 2));

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
            TestEntity[] result = await mockUnitTestSpecification.GetAll(entities.AsQueryable(), new TestEntityOrderByNameAscSpecification());
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
            TestEntity[] result = await mockUnitTestSpecification.GetAll(entities.AsQueryable(), new TestEntityOrderByNameDescSpecification());
            TestEntity[] orderedList = entities.OrderByDescending(o => o.Name).ToArray();

            // Assert
            result.Should().Equals(orderedList);
        }

        /// <summary>
        /// The Should_SetCriteria.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_SetCriteria()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] result = await mockUnitTestSpecification.GetAll(entities.AsQueryable(), new TestEntityByIdSpecification(entities[0].Id));

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(entities[0]);
        }

        [Fact]
        public async Task Should_SetDistinct()
        {
            // Arrange
            IEnumerable<TestEntity> entities = _fixture.CreateMany<TestEntity>(4);
            var groupedEntities = new List<TestEntity>();
            groupedEntities.AddRange(entities.ToList());
            groupedEntities.AddRange(entities.ToList());

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] result = await mockUnitTestSpecification.GetAll(entities.AsQueryable(), new TestEntityDistinctSpecification());

            // Assert
            result.Should().HaveCount(4);
        }

        /// <summary>
        /// The CharactersOfTypeHumanSpecification_ShouldApplySpecification.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task TestEntityWithChildEntitiesSpecification_ShouldApplySpecification()
        {
            // Arrange
            var parentGuid = Guid.NewGuid();
            TestEntityChild childEntity = _fixture.Build<TestEntityChild>()
                .Without(wh => wh.Parent)
                .With(wh => wh.ParentId, parentGuid)
                .Create();
            IEnumerable<TestEntity> testEntitiesWithChild = _fixture.Build<TestEntity>()
                .With(w => w.Id, parentGuid)
                .With(w => w.Children, new List<TestEntityChild> { childEntity })
                .CreateMany(3);

            IEnumerable<TestEntity> testEntitiesWithoutChildren = _fixture.Build<TestEntity>()
                .Without(wh => wh.Children)
                .CreateMany(5);

            var testEntities = new List<TestEntity>();
            testEntities.AddRange(testEntitiesWithoutChildren);
            testEntities.AddRange(testEntitiesWithChild);

            var mockUnitTestSpecification = new SpecificationRepository<TestEntity, Guid>();

            // Act
            TestEntity[] testEntityResults = await mockUnitTestSpecification.GetAll(testEntities.AsQueryable(), new TestEntityWithChildEntitiesSpecification());

            // Assert
            testEntityResults.Should().NotBeNull().And.HaveSameCount(testEntitiesWithChild);
        }
    }
}
