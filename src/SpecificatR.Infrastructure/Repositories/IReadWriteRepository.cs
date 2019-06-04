// <copyright file="IReadWriteRepository.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Repositories
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;

    public interface IReadWriteRepository<TEntity, TIdentifier, TDbContext> : IReadRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task DeleteByIdAsync(TIdentifier id);

        Task UpdateAsync(TEntity entity);

        Task UpdateFieldsAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    }
}
