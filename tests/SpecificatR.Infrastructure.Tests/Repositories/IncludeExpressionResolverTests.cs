using FluentAssertions;
using SpecificatR.Infrastructure.Internal;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SpecificatR.Infrastructure.Tests.Repositories
{
    public class IncludeExpressionResolverTests
    {
        [Fact]
        public void Resolve_DirectProperty_ShouldReturnNameOfProperty()
        {
            // Act
            var include = IncludeExpressionResolver.Resolve<Entity>(x => x.Name);

            // Assert
            include.Should().Be("Name");
        }

        [Fact]
        public void Resolve_OneToMany_NestedNestedProperty_ShouldReturnDottedPathToNestedProperty()
        {
            // Act
            var include = IncludeExpressionResolver.Resolve<Entity>(x => x.NestedItems.Select(y => y.NestedNestedItems.Select(z => z.NestedNestedName)));

            // Assert
            include.Should().Be("NestedItems.NestedNestedItems.NestedNestedName");
        }

        [Fact]
        public void Resolve_OneToMany_NestedProperty_ShouldReturnDottedPathToNestedProperty()
        {
            // Act
            var include = IncludeExpressionResolver.Resolve<Entity>(x => x.NestedItems.Select(y => y.NestedName));

            // Assert
            include.Should().Be("NestedItems.NestedName");
        }

        [Fact]
        public void Resolve_OneToOne_NestedProperty_ShouldReturnDottedPathToNestedProperty()
        {
            // Act
            var include = IncludeExpressionResolver.Resolve<Entity>(x => x.NestedItem.NestedName);

            // Assert
            include.Should().Be("NestedItem.NestedName");
        }

        private sealed class Entity
        {
            public string Name { get; set; }

            public NestedEntity NestedItem { get; set; }

            public ICollection<NestedEntity> NestedItems { get; set; }
        }

        private sealed class NestedEntity
        {
            public string NestedName { get; set; }

            public ICollection<NestedNestedEntity> NestedNestedItems { get; set; }
        }

        private sealed class NestedNestedEntity
        {
            public string NestedNestedName { get; set; }
        }
    }
}
