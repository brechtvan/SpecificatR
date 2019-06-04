// <copyright file="IReadRepository.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;

    public interface IReadRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        Task<TEntity[]> GetAllAsync(bool asTracking = false);

        Task<TEntity[]> GetAllAsync(ISpecification<TEntity> specification);

        Task<TEntity> GetByIdAsync(TIdentifier id, bool asTracking = false);

        Task<TEntity> GetSingleWithSpecificationAsync(ISpecification<TEntity> specification);
    }
}
