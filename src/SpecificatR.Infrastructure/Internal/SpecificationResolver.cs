//-----------------------------------------------------------------------
// <copyright file="SpecificationResolver.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Internal
{
    using System.Linq;
    using SpecificatR.Infrastructure.Abstractions;

    internal class SpecificationResolver<TEntity, TIdentifier>
         where TEntity : class, IBaseEntity<TIdentifier>
    {
        public static int GetCount(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            return ApplySpecification(inputQuery: inputQuery, specification: specification).Count();
        }

        public static TEntity[] GetResultSet(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            return ApplySpecification(inputQuery: inputQuery, specification: specification).ToArray();
        }

        public static (TEntity[] entities, int filteredCount) GetResultSetWithCount(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            (IQueryable<TEntity> queryableEntities, int filteredCount) = ApplySpecificationWithCount(inputQuery: inputQuery, specification: specification);

            return (entities: queryableEntities.ToArray(), filteredCount);
        }

        public static TEntity GetSingleResultAsync(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            return ApplySpecification(inputQuery: inputQuery, specification: specification).FirstOrDefault();
        }

        protected static IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity, TIdentifier>.GetQuery(inputQuery: inputQuery, specification: specification);
        }

        protected static (IQueryable<TEntity> queryableEntities, int filteredCount) ApplySpecificationWithCount(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity, TIdentifier>.GetQueryWithCount(inputQuery: inputQuery, specification: specification);
        }
    }
}
