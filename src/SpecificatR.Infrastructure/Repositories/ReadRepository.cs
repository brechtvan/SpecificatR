// <copyright file="ReadRepository.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;
    using SpecificatR.Infrastructure.Internal;

    internal class ReadRepository<TEntity, TIdentifier, TDbContext> : IReadRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;

        public ReadRepository(TDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity[]> GetAllAsync(bool asTracking = false)
        {
            if (asTracking)
            {
                return await _context.Set<TEntity>().ToArrayAsync();
            }

            return await _context.Set<TEntity>().AsNoTracking().ToArrayAsync();
        }

        public async Task<TEntity[]> GetAllAsync(ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetResultSet(_context.Set<TEntity>().AsQueryable(), specification));
        }

        public async Task<(TEntity[] entities, int filteredCount)> GetAllWithCountAsync(ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetResultSetWithCount(_context.Set<TEntity>().AsQueryable(), specification));
        }

        public async Task<TEntity> GetByIdAsync(TIdentifier id, bool asTracking = false)
        {
            if (asTracking)
            {
                return await _context.Set<TEntity>().FirstOrDefaultAsync(fod => fod.Id.Equals(id));
            }

            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(fod => fod.Id.Equals(id));
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetCount(_context.Set<TEntity>().AsQueryable(), specification));
        }

        public async Task<TEntity> GetSingleWithSpecificationAsync(ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetSingleResultAsync(_context.Set<TEntity>().AsQueryable(), specification));
        }
    }
}
