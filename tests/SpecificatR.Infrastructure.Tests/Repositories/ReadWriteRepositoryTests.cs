using AutoFixture;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Repositories;
using SpecificatR.Infrastructure.Tests;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Generic.Infrastructure.Sql.Tests
{
    public class ReadWriteRepositoryTests
    {
        private readonly IFixture _fixture = new Fixture();

        private readonly DbContextOptions<TestDbContext> _options = new DbContextOptions<TestDbContext>();

        [Fact]
        public async Task AddEntity_ShouldAddEntity()
        {
            // Arrange
            TestEntity entity = _fixture.Create<TestEntity>();
            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id));

            var repository = new ReadWriteRepository<TestEntity, Guid>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.AddAsync(entity);
            await repository.CommitAsync();

            // Assert
            result.Id.Should().Be(entity.Id);
            result.Name.Should().Be(entity.Name);
        }

        [Fact]
        public async Task DeleteAsync_KnownEntity_ShouldDeleteEntity()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();
            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);

            var repository = new ReadWriteRepository<TestEntity, Guid>(dbContextMock.Object);

            // Act
            await repository.DeleteAsync(entities[0]);
            await repository.CommitAsync();
            TestEntity[] getAll = await repository.GetAllAsync();

            // Assert
            getAll.SingleOrDefault(x => x.Id == entities[0].Id).Should().BeNull();
            getAll.Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteAsync_UnknownEntity_ShouldNotThrowException()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();
            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);

            var repository = new ReadWriteRepository<TestEntity, Guid>(dbContextMock.Object);

            // Act
            await repository.DeleteAsync(_fixture.Create<TestEntity>());
            await repository.CommitAsync();
            TestEntity[] getAll = await repository.GetAllAsync();

            // Assert
            getAll.Should().ContainEquivalentOf(entities[0]);
            getAll.Should().HaveCount(2);
        }

        [Fact]
        public async Task DeleteByIdAsync_KnownEntity_ShouldDeleteEntity()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);
            var repository = new ReadWriteRepository<TestEntity, Guid>(dbContextMock.Object);

            // Act
            await repository.DeleteByIdAsync(entities[0].Id);
            await repository.CommitAsync();
            TestEntity[] getAll = await repository.GetAllAsync();

            // Assert
            getAll.SingleOrDefault(x => x.Id == entities[0].Id).Should().BeNull();
            getAll.Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteByIdAsync_UnknownEntity_ShouldThrowException()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => (x.Id), entities);
            var repository = new ReadWriteRepository<TestEntity, Guid>(dbContextMock.Object);

            // Act
            Func<Task> deleteEntity = async () => await repository.DeleteByIdAsync(Guid.NewGuid());

            // Assert
            FluentAssertions.Specialized.ExceptionAssertions<NullReferenceException> result = await deleteEntity.Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task UpdateAsync_ExistingEntity_ShouldUpdateEntity()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Luke Skywalker",
                Number = 1
            };

            DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
               .UseInMemoryDatabase(databaseName: "InMemory_TestDb")
               .Options;

            var dbContext = new TestDbContext(options);
            var repository = new ReadWriteRepository<TestEntity, Guid>(dbContext);

            dbContext.Add(testEntity);
            await dbContext.SaveChangesAsync();
            testEntity.Name = "Han Solo";
            testEntity.Number = 2;

            // Act
            await repository.UpdateAsync(testEntity);
            var result = await repository.CommitAsync();
            TestEntity verifyEntity = await repository.GetByIdAsync(testEntity.Id);

            // Assert
            result.Should().Be(1);
            verifyEntity.Name.Should().Be("Han Solo");
            verifyEntity.Number.Should().Be(2);
        }

        [Fact]
        public async Task UpdateField_WithoutProperties_ShouldReturnException()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Yoda",
                Number = 1
            };

            DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
               .UseInMemoryDatabase(databaseName: "InMemory_TestDb")
               .Options;

            var dbContext = new TestDbContext(options);
            var repository = new ReadWriteRepository<TestEntity, Guid>(dbContext);

            dbContext.Add(testEntity);
            await dbContext.SaveChangesAsync();
            testEntity.Name = "Mace Windu";

            // Act
            Func<Task> result = async () => await repository.UpdateFieldsAsync(testEntity, null);

            // Assert
            result.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public async Task UpdateFieldsAsync_WithMultipleChangedEntities_ShouldOnlyUpdateSpecifiedEntity()
        {
            // Arrange
            var testEntity1 = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Count Dooku",
                Number = 1
            };
            var testEntity2 = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Kit Fisto",
                Number = 2
            };

            DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_TestDb")
                .Options;

            using (var dbContext = new TestDbContext(options))
            {
                dbContext.Add(testEntity1);
                dbContext.Add(testEntity2);
                await dbContext.SaveChangesAsync();
            }

            // Act
            using (var dbContext = new TestDbContext(options))
            {
                var repository = new ReadWriteRepository<TestEntity, Guid>(dbContext);
                testEntity1 = await repository.GetByIdAsync(testEntity1.Id);
                testEntity1.Name = "Darth Vader";
                testEntity1.Number = 45;
                testEntity2.Name = "Sheev Palpatine";

                await repository.UpdateFieldsAsync(testEntity1, te => te.Name);
                var result = await repository.CommitAsync();

                // Assert
                result.Should().Be(1);
            }

            // Assert
            using (var dbContext = new TestDbContext(options))
            {
                var repository = new ReadWriteRepository<TestEntity, Guid>(dbContext);
                TestEntity updatedTestEntity1 = await repository.GetByIdAsync(testEntity1.Id);
                TestEntity updatedTestEntity2 = await repository.GetByIdAsync(testEntity2.Id);

                updatedTestEntity1.Name.Should().Be("Darth Vader");
                updatedTestEntity1.Number.Should().Be(1);
                updatedTestEntity2.Name.Should().Be("Kit Fisto");
            }
        }

        [Fact]
        public async Task UpdateFieldsAsync_WithProperties_ShouldOnlyUpdateSpecifiedEntityProperties()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Anakin Skywalker",
                Number = 1
            };

            DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_TestDb")
                .Options;

            using (var dbContext = new TestDbContext(options))
            {
                dbContext.Add(testEntity);
                await dbContext.SaveChangesAsync();
            }

            testEntity.Name = "Obi-Wan Kenobi";
            testEntity.Number = 2;

            // Act
            using (var dbContext = new TestDbContext(options))
            {
                var repository = new ReadWriteRepository<TestEntity, Guid>(dbContext);

                await repository.UpdateFieldsAsync(testEntity, te => te.Name);
                var result = await repository.CommitAsync();
                // Assert
                result.Should().Be(1);
            }

            // Assert
            using (var dbContext = new TestDbContext(options))
            {
                var repository = new ReadWriteRepository<TestEntity, Guid>(dbContext);
                TestEntity updatedTestEntity = await repository.GetByIdAsync(testEntity.Id);
                updatedTestEntity.Name.Should().Be("Obi-Wan Kenobi");
                updatedTestEntity.Number.Should().Be(1);
            }
        }

        [Fact]
        public async Task UpdateFieldsAsync_WithProperties_ShouldUpdateEntityProperties()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Chewbacca",
                Number = 1
            };

            DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_TestDb")
                .Options;

            using (var dbContext = new TestDbContext(options))
            {
                dbContext.Add(testEntity);
                await dbContext.SaveChangesAsync();
            }

            TestEntity updatedTestEntity = null;

            testEntity.Name = "R2-D2";
            testEntity.Number = 2;

            // Act
            using (var dbContext = new TestDbContext(options))
            {
                var repository = new ReadWriteRepository<TestEntity, Guid>(dbContext);
                await repository.UpdateFieldsAsync(testEntity, te => te.Name, te => te.Number);
                var result = await repository.CommitAsync();

                // Assert
                result.Should().Be(1);
                updatedTestEntity = await repository.GetByIdAsync(testEntity.Id);
            }

            // Assert
            updatedTestEntity.Name.Should().Be("R2-D2");
            updatedTestEntity.Number.Should().Be(2);
        }

        [Fact]
        public async Task UpdateFieldsAsync_WithProperties_ShouldUpdateSpecifiedEntityProperty()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Kylo Ren",
                Number = 1
            };

            DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
               .UseInMemoryDatabase(databaseName: "InMemory_TestDb")
               .Options;

            using (var dbContext = new TestDbContext(options))
            {
                dbContext.Add(testEntity);
                await dbContext.SaveChangesAsync();
            }

            testEntity.Name = "Rey Skywalker";
            TestEntity updatedTeamMember = null;
            using (var dbContext = new TestDbContext(options))
            {
                var repository = new ReadWriteRepository<TestEntity, Guid>(dbContext);

                // Act
                await repository.UpdateFieldsAsync(testEntity, te => te.Name);
                var result = await repository.CommitAsync();

                // Assert
                result.Should().Be(1);
                updatedTeamMember = await repository.GetByIdAsync(testEntity.Id);
            }

            // Assert
            updatedTeamMember.Name.Should().Be("Rey Skywalker");
        }
    }
}
