// <copyright file="SpecificationTests.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;
    using SpecificatR.Infrastructure.UnitTest.Abstractions;
    using Xunit;

    public class SpecificationTests
    {
        private readonly IFixture _fixture = new Fixture();

        private readonly DbContextOptions<TestDbContext> _options = new DbContextOptions<TestDbContext>();

        public SpecificationTests()
        {
            _fixture.Customize<TestEntity>(te => te.Without(w => w.Children));
        }

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
            TestEntity[] testEntityResults = await mockUnitTestSpecification.GetAllAsync(testEntities.AsQueryable(), new TestEntityWithChildEntitiesSpecification());

            // Assert
            testEntityResults.Should().NotBeNull().And.HaveSameCount(testEntitiesWithChild);
        }

        private class TestEntityWithChildEntitiesSpecification : BaseSpecification<TestEntity>
        {
            public TestEntityWithChildEntitiesSpecification()
                : base(BuildCriteria())
            {
            }

            private static Expression<Func<TestEntity, bool>> BuildCriteria()
            {
                return x => x.Children != null && x.Children.Any();
            }
        }

        private class TestEntityByIdSpecification : BaseSpecification<TestEntity>
        {
            public TestEntityByIdSpecification(Guid id)
                : base(BuildCriteria(id))
            {
            }

            private static Expression<Func<TestEntity, bool>> BuildCriteria(Guid id)
                => x => x.Id == id;
        }

        private class TestEntityOrderByNameAscSpecification : BaseSpecification<TestEntity>
        {
            public TestEntityOrderByNameAscSpecification()
                : base(null)
            {
                AddOrderBy(o => o.Name, OrderByDirection.Ascending);
            }
        }

        private class TestEntityOrderByNameDescSpecification : BaseSpecification<TestEntity>
        {
            public TestEntityOrderByNameDescSpecification()
                : base(null)
            {
                AddOrderBy(o => o.Name, OrderByDirection.Descending);
            }
        }

        private class TestEntityPaginatedSpecification : BaseSpecification<TestEntity>
        {
            public TestEntityPaginatedSpecification(int pageIndex, int pageSize)
                : base(null)
            {
                ApplyPaging(pageIndex, pageSize);
            }
        }
    }
}
