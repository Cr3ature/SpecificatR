//-----------------------------------------------------------------------
// <copyright file="ReadWriteRepositoryTests.cs" company="David Vanderheyden">
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
    using SpecificatR.Infrastructure.Repositories;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="ReadWriteBaseRepositoryTests" />.
    /// </summary>
    public class ReadWriteBaseRepositoryTests
    {
        /// <summary>
        /// Defines the _fixture.
        /// </summary>
        private readonly IFixture _fixture = new Fixture();

        /// <summary>
        /// Defines the _options.
        /// </summary>
        private readonly DbContextOptions<TestDbContext> _options = new DbContextOptions<TestDbContext>();

        public ReadWriteBaseRepositoryTests()
        {
            _fixture.Customize<TestEntity>(testEntity => testEntity.Without(w => w.Children));
        }

        /// <summary>
        /// The AddEntity_ShouldAddEntity.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task AddEntity_ShouldAddEntity()
        {
            // Arrange
            TestEntity entity = _fixture.Create<TestEntity>();
            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id);

            var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            TestEntity result = await repository.Add(entity);

            // Assert
            result.Id.Should().Be(entity.Id);
            result.Name.Should().Be(entity.Name);
        }

        /// <summary>
        /// The DeleteAsync_KnownEntity_ShouldDeleteEntity.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task DeleteAsync_KnownEntity_ShouldDeleteEntity()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();
            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);

            var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            await repository.Delete(entities[0]);
            await repository.CommitAsync();
            TestEntity[] getAll = await repository.GetAll();

            // Assert
            getAll.SingleOrDefault(x => x.Id == entities[0].Id).Should().BeNull();
            getAll.Should().HaveCount(1);
        }

        /// <summary>
        /// The DeleteAsync_UnknownEntity_ShouldNotThrowException.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task DeleteAsync_UnknownEntity_ShouldNotThrowException()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();
            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);

            var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            await repository.Delete(_fixture.Create<TestEntity>());
            await repository.CommitAsync();
            TestEntity[] getAll = await repository.GetAll();

            // Assert
            getAll.Should().ContainEquivalentOf(entities[0]);
            getAll.Should().HaveCount(2);
        }

        /// <summary>
        /// The DeleteByIdAsync_KnownEntity_ShouldDeleteEntity.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task DeleteByIdAsync_KnownEntity_ShouldDeleteEntity()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);
            var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            await repository.DeleteById(entities[0].Id);
            await repository.CommitAsync();
            TestEntity[] getAll = await repository.GetAll();

            // Assert
            getAll.SingleOrDefault(x => x.Id == entities[0].Id).Should().BeNull();
            getAll.Should().HaveCount(1);
        }

        /// <summary>
        /// The DeleteByIdAsync_UnknownEntity_ShouldThrowException.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task DeleteByIdAsync_UnknownEntity_ShouldThrowException()
        {
            // Arrange
            TestEntity[] entities = _fixture.CreateMany<TestEntity>(2).ToArray();

            var dbContextMock = new DbContextMock<TestDbContext>(_options);
            dbContextMock.CreateDbSetMock(x => x.TestEntities, (x, _) => x.Id, entities);
            var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContextMock.Object);

            // Act
            Func<Task> deleteEntity = async () => await repository.DeleteById(Guid.NewGuid());

            // Assert
            FluentAssertions.Specialized.ExceptionAssertions<NullReferenceException> result = await deleteEntity.Should().ThrowAsync<NullReferenceException>();
        }

        /// <summary>
        /// The UpdateAsync_ExistingEntity_ShouldUpdateEntity.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task UpdateAsync_ExistingEntity_ShouldUpdateEntity()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Luke Skywalker",
                Number = 1,
            };

            DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
               .UseInMemoryDatabase(databaseName: "InMemory_TestDb")
               .Options;

            var dbContext = new TestDbContext(options);
            var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContext);

            dbContext.Add(testEntity);
            await dbContext.SaveChangesAsync();
            testEntity.Name = "Han Solo";
            testEntity.Number = 2;

            // Act
            await repository.Update(testEntity);
            TestEntity verifyEntity = await repository.GetById(testEntity.Id);

            // Assert
            verifyEntity.Name.Should().Be("Han Solo");
            verifyEntity.Number.Should().Be(2);
        }

        /// <summary>
        /// The UpdateField_WithoutProperties_ShouldReturnException.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task UpdateField_WithoutProperties_ShouldReturnException()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Yoda",
                Number = 1,
            };

            DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
               .UseInMemoryDatabase(databaseName: "InMemory_TestDb")
               .Options;

            var dbContext = new TestDbContext(options);
            var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContext);

            dbContext.Add(testEntity);
            await dbContext.SaveChangesAsync();
            testEntity.Name = "Mace Windu";

            // Act
            Func<Task> result = async () => await repository.UpdateFields(testEntity, null);

            // Assert
            result.Should().Throw<NullReferenceException>();
        }

        /// <summary>
        /// The UpdateFieldsAsync_WithMultipleChangedEntities_ShouldOnlyUpdateSpecifiedEntity.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task UpdateFieldsAsync_WithMultipleChangedEntities_ShouldOnlyUpdateSpecifiedEntity()
        {
            // Arrange
            var testEntity1 = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Count Dooku",
                Number = 1,
            };
            var testEntity2 = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Kit Fisto",
                Number = 2,
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
                var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContext);
                testEntity1 = await repository.GetById(testEntity1.Id);
                testEntity1.Name = "Darth Vader";
                testEntity1.Number = 45;
                testEntity2.Name = "Sheev Palpatine";

                await repository.UpdateFields(testEntity1, te => te.Name);
            }

            // Assert
            using (var dbContext = new TestDbContext(options))
            {
                var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContext);
                TestEntity updatedTestEntity1 = await repository.GetById(testEntity1.Id);
                TestEntity updatedTestEntity2 = await repository.GetById(testEntity2.Id);

                updatedTestEntity1.Name.Should().Be("Darth Vader");
                updatedTestEntity1.Number.Should().Be(1);
                updatedTestEntity2.Name.Should().Be("Kit Fisto");
            }
        }

        /// <summary>
        /// The UpdateFieldsAsync_WithProperties_ShouldOnlyUpdateSpecifiedEntityProperties.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task UpdateFieldsAsync_WithProperties_ShouldOnlyUpdateSpecifiedEntityProperties()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Anakin Skywalker",
                Number = 1,
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
                var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContext);

                await repository.UpdateFields(testEntity, te => te.Name);
            }

            // Assert
            using (var dbContext = new TestDbContext(options))
            {
                var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContext);
                TestEntity updatedTestEntity = await repository.GetById(testEntity.Id);
                updatedTestEntity.Name.Should().Be("Obi-Wan Kenobi");
                updatedTestEntity.Number.Should().Be(1);
            }
        }

        /// <summary>
        /// The UpdateFieldsAsync_WithProperties_ShouldUpdateEntityProperties.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task UpdateFieldsAsync_WithProperties_ShouldUpdateEntityProperties()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Chewbacca",
                Number = 1,
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
                var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContext);
                await repository.UpdateFields(testEntity, te => te.Name, te => te.Number);

                // Assert
                updatedTestEntity = await repository.GetById(testEntity.Id);
            }

            // Assert
            updatedTestEntity.Name.Should().Be("R2-D2");
            updatedTestEntity.Number.Should().Be(2);
        }

        /// <summary>
        /// The UpdateFieldsAsync_WithProperties_ShouldUpdateSpecifiedEntityProperty.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task UpdateFieldsAsync_WithProperties_ShouldUpdateSpecifiedEntityProperty()
        {
            // Arrange
            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "Kylo Ren",
                Number = 1,
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
                var repository = new ReadWriteBaseRepository<TestEntity, Guid, TestDbContext>(dbContext);

                // Act
                await repository.UpdateFields(testEntity, te => te.Name);

                // Assert
                updatedTeamMember = await repository.GetById(testEntity.Id);
            }

            // Assert
            updatedTeamMember.Name.Should().Be("Rey Skywalker");
        }
    }
}
