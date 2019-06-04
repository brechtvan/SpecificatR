// <copyright file="TestDbContext.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TestEntity> TestEntities { get; set; }

        public virtual DbSet<TestEntityChild> TestEntityChildren { get; set; }
    }

    public class TestEntity : IBaseEntity<Guid>
    {
        public virtual ICollection<TestEntityChild> Children { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }
    }

    public class TestEntityChild : IBaseEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TestEntity Parent { get; set; }

        public Guid ParentId { get; set; }
    }
}
