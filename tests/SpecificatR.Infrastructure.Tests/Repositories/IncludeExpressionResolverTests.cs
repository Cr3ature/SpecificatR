//-----------------------------------------------------------------------
// <copyright file="IncludeExpressionResolverTests.cs">
//     Copyright (c) 2019-2020 David Vanderheyden All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using SpecificatR.Infrastructure.Internal;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="IncludeExpressionResolverTests"/>.
    /// </summary>
    public class IncludeExpressionResolverTests
    {
        /// <summary>
        /// The Resolve_DirectProperty_ShouldReturnNameOfProperty.
        /// </summary>
        [Fact]
        public void Resolve_DirectProperty_ShouldReturnNameOfProperty()
        {
            // Act
            var include = IncludeExpressionResolver.Resolve<Entity>(x => x.Name);

            // Assert
            include.Should().Be("Name");
        }

        /// <summary>
        /// The Resolve_OneToMany_NestedNestedProperty_ShouldReturnDottedPathToNestedProperty.
        /// </summary>
        [Fact]
        public void Resolve_OneToMany_NestedNestedProperty_ShouldReturnDottedPathToNestedProperty()
        {
            // Act
            var include = IncludeExpressionResolver.Resolve<Entity>(x => x.NestedItems.Select(y => y.NestedNestedItems.Select(z => z.NestedNestedName)));

            // Assert
            include.Should().Be("NestedItems.NestedNestedItems.NestedNestedName");
        }

        /// <summary>
        /// The Resolve_OneToMany_NestedProperty_ShouldReturnDottedPathToNestedProperty.
        /// </summary>
        [Fact]
        public void Resolve_OneToMany_NestedProperty_ShouldReturnDottedPathToNestedProperty()
        {
            // Act
            var include = IncludeExpressionResolver.Resolve<Entity>(x => x.NestedItems.Select(y => y.NestedName));

            // Assert
            include.Should().Be("NestedItems.NestedName");
        }

        /// <summary>
        /// The Resolve_OneToOne_NestedProperty_ShouldReturnDottedPathToNestedProperty.
        /// </summary>
        [Fact]
        public void Resolve_OneToOne_NestedProperty_ShouldReturnDottedPathToNestedProperty()
        {
            // Act
            var include = IncludeExpressionResolver.Resolve<Entity>(x => x.NestedItem.NestedName);

            // Assert
            include.Should().Be("NestedItem.NestedName");
        }

        /// <summary>
        /// Defines the <see cref="Entity"/>.
        /// </summary>
        private sealed class Entity
        {
            /// <summary>
            /// Gets or sets the Name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the NestedItem.
            /// </summary>
            public NestedEntity NestedItem { get; set; }

            /// <summary>
            /// Gets or sets the NestedItems.
            /// </summary>
            public ICollection<NestedEntity> NestedItems { get; set; }
        }

        /// <summary>
        /// Defines the <see cref="NestedEntity"/>.
        /// </summary>
        private sealed class NestedEntity
        {
            /// <summary>
            /// Gets or sets the NestedName.
            /// </summary>
            public string NestedName { get; set; }

            /// <summary>
            /// Gets or sets the NestedNestedItems.
            /// </summary>
            public ICollection<NestedNestedEntity> NestedNestedItems { get; set; }
        }

        /// <summary>
        /// Defines the <see cref="NestedNestedEntity"/>.
        /// </summary>
        private sealed class NestedNestedEntity
        {
            /// <summary>
            /// Gets or sets the NestedNestedName.
            /// </summary>
            public string NestedNestedName { get; set; }
        }
    }
}
