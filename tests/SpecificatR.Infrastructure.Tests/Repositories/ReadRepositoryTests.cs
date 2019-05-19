using AutoFixture;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpecificatR.Infrastructure.Abstractions;
using SpecificatR.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpecificatR.Infrastructure.Tests.Repositories
{
    public class ReadRepositoryTests
    {
        private readonly IFixture _fixture = new Fixture();

        private readonly DbContextOptions<TestDbContext> _options = new DbContextOptions<TestDbContext>();

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);

            var repository = new ReadRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.GetByIdAsync(entities[0].Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(entities[0]);
        }

        [Fact]
        public async Task GetByIdAsync_WithTracking_ShouldReturnEntity()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);

            var repository = new ReadRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.GetByIdAsync(entities[0].Id, true);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(entities[0]);
        }

        [Fact]
        public async Task GetByIdAsync_UnknownId_ShouldReturnNull()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);
            var repository = new ReadRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_UnknownIdAndWithTracking_ShouldReturnNull()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);
            var repository = new ReadRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.GetByIdAsync(Guid.NewGuid(), true);

            // Assert
            result.Should().BeNull();
        }


        [Fact]
        public async Task GetByIdAsyncWithSpecification_ShouldApplySpecification()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);
            var repository = new ReadRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            var specification = new Mock<ISpecification<TestEntity>>();

            // Act
            TestEntity result = await repository.GetSingleWithSpecificationAsync(specification.Object);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(TestEntity));
        }

        [Fact]
        public async Task ListAllAsync_ShouldReturnEntities()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);
            var repository = new ReadRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity[] result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task ListAllAsync_WithTracking_ShouldReturnEntities()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);
            var repository = new ReadRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity[] result = await repository.GetAllAsync(true);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task ListAllAsyncWithSpecification_ShouldApplySpecification()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);
            var repository = new ReadRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            var specification = new Mock<ISpecification<TestEntity>>();

            // Act
            TestEntity[] result = await repository.GetAllAsync(specification.Object);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(TestEntity[]));
        }
    }
}
