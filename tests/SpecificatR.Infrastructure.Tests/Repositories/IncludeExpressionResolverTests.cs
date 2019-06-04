// <copyright file="IncludeExpressionResolverTests.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using SpecificatR.Infrastructure.Internal;
    using Xunit;

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
