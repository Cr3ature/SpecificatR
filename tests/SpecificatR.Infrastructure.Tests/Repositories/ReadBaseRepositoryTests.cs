//-----------------------------------------------------------------------
// <copyright file="ReadRepositoryTests.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:48</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Tests.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using EntityFrameworkCoreMock;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SpecificatR.Infrastructure.Abstractions;
    using SpecificatR.Infrastructure.Repositories;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="ReadBaseRepositoryTests" />.
    /// </summary>
    public class ReadBaseRepositoryTests
    {
        /// <summary>
        /// Defines the _fixture.
        /// </summary>
        private readonly IFixture _fixture = new Fixture();

        /// <summary>
        /// Defines the _options.
        /// </summary>
        private readonly DbContextOptions<TestDbContext> _options = new DbContextOptions<TestDbContext>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadBaseRepositoryTests"/> class.
        /// </summary>
        public ReadBaseRepositoryTests()
        {
            _fixture.Customize<TestEntity>(testEntity => testEntity.Without(w => w.Children));
        }

        /// <summary>
        /// The GetByIdAsync_ShouldReturnEntity.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);

            var repository = new ReadBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.GetById(entities[0].Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(entities[0]);
        }

        /// <summary>
        /// The GetByIdAsync_UnknownId_ShouldReturnNull.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetByIdAsync_UnknownId_ShouldReturnNull()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);
            var repository = new ReadBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.GetById(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// The GetByIdAsync_UnknownIdAndWithTracking_ShouldReturnNull.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetByIdAsync_UnknownIdAndWithTracking_ShouldReturnNull()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);
            var repository = new ReadBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.GetById(Guid.NewGuid(), true);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// The GetByIdAsync_WithTracking_ShouldReturnEntity.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetByIdAsync_WithTracking_ShouldReturnEntity()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);

            var repository = new ReadBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.GetById(entities[0].Id, true);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(entities[0]);
        }

        /// <summary>
        /// The GetByIdAsyncWithSpecification_ShouldApplySpecification.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetByIdAsyncWithSpecification_ShouldApplySpecification()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);
            var repository = new ReadBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            var specification = new Mock<ISpecification<TestEntity>>();

            // Act
            TestEntity result = await repository.GetSingleWithSpecification(specification.Object);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(TestEntity));
        }

        /// <summary>
        /// The ListAllAsync_ShouldReturnEntities.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task ListAllAsync_ShouldReturnEntities()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);
            var repository = new ReadBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity[] result = await repository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        /// <summary>
        /// The ListAllAsync_WithTracking_ShouldReturnEntities.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task ListAllAsync_WithTracking_ShouldReturnEntities()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);
            var repository = new ReadBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity[] result = await repository.GetAll(true);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        /// <summary>
        /// The ListAllAsyncWithSpecification_ShouldApplySpecification.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task ListAllAsyncWithSpecification_ShouldApplySpecification()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);
            var repository = new ReadBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            var specification = new Mock<ISpecification<TestEntity>>();

            // Act
            TestEntity[] result = await repository.GetAll(specification.Object);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(TestEntity[]));
        }
    }
}
