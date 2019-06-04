// <copyright file="SpecificationRepository.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.UnitTest.Abstractions
{
    using System.Linq;
    using System.Threading.Tasks;
    using SpecificatR.Infrastructure.Abstractions;
    using SpecificatR.Infrastructure.Internal;

    public class SpecificationRepository<TEntity, TIdentifier>
        where TEntity : class, IBaseEntity<TIdentifier>
    {
        public async Task<TEntity[]> GetAllAsync(IQueryable<TEntity> queryable, ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetResultSet(
                inputQuery: queryable,
                specification: specification));
        }

        public async Task<int> GetCountAsync(IQueryable<TEntity> queryable, ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetCount(
                inputQuery: queryable,
                specification: specification));
        }

        public async Task<TEntity> GetSingleWithSpecificationAsync(IQueryable<TEntity> queryable, ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetSingleResultAsync(
                inputQuery: queryable,
                specification: specification));
        }

        public async Task<(TEntity[] entities, int filteredCount)> GetAllWithCountAsync(IQueryable<TEntity> queryable, ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetResultSetWithCount(
                inputQuery: queryable,
                specification: specification));
        }
    }
}
