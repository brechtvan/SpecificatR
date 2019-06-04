//-----------------------------------------------------------------------
// <copyright file="SpecificationEvaluator.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Internal
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;

    internal class SpecificationEvaluator<TEntity, TIdentifier>
        where TEntity : class, IBaseEntity<TIdentifier>
    {
        internal static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            IQueryable<TEntity> outputQuery = inputQuery;

            outputQuery = SetCriteria(outputQuery, specification.Criteria);

            outputQuery = SetIncludes(outputQuery, specification);

            outputQuery = SetPaging(outputQuery, specification);

            outputQuery = SetOrderBy(outputQuery, specification);

            outputQuery = SetIgnoreQueryFilters(outputQuery, specification);

            outputQuery = SetTracking(outputQuery, specification);

            return outputQuery;
        }

        internal static (IQueryable<TEntity> query, int filteredCount) GetQueryWithCount(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            IQueryable<TEntity> outputQuery = inputQuery;

            outputQuery = SetCriteria(outputQuery, specification.Criteria);

            outputQuery = SetIncludes(outputQuery, specification);

            var filteredCount = outputQuery.Count();

            outputQuery = SetPaging(outputQuery, specification);

            outputQuery = SetOrderBy(outputQuery, specification);

            outputQuery = SetIgnoreQueryFilters(outputQuery, specification);

            outputQuery = SetTracking(outputQuery, specification);

            return (outputQuery, filteredCount);
        }

        private static IQueryable<TEntity> SetCriteria(IQueryable<TEntity> outputQuery, Expression<Func<TEntity, bool>> criteria)
        {
            if (criteria == null)
            {
                return outputQuery;
            }

            return outputQuery.Where(criteria);
        }

        private static IQueryable<TEntity> SetIgnoreQueryFilters(IQueryable<TEntity> outputQuery, ISpecification<TEntity> specification)
        {
            if (specification.IgnoreQueryFilters)
            {
                return outputQuery.IgnoreQueryFilters();
            }

            return outputQuery;
        }

        private static IQueryable<TEntity> SetIncludes(IQueryable<TEntity> outputQuery, ISpecification<TEntity> specification)
        {
            if (specification.Includes == null || !specification.Includes.Any())
            {
                return outputQuery;
            }

            foreach (Expression<Func<TEntity, object>> argument in specification.Includes)
            {
                string include = IncludeExpressionResolver.Resolve(argument);
                outputQuery = outputQuery.Include(include);
            }

            return outputQuery;
        }

        private static IQueryable<TEntity> SetOrderBy(IQueryable<TEntity> outputQuery, ISpecification<TEntity> specification)
        {
            if (specification.OrderByExpressions == null || !specification.OrderByExpressions.Any())
            {
                return outputQuery;
            }

            OrderByExpression<TEntity> firstOrderByExpression = specification.OrderByExpressions.First();

            IOrderedQueryable<TEntity> orderedQuery = firstOrderByExpression.OrderByDirection.Equals(OrderByDirection.Ascending)
                ? outputQuery.OrderBy(firstOrderByExpression.Expression)
                : outputQuery.OrderByDescending(firstOrderByExpression.Expression);

            foreach (OrderByExpression<TEntity> orderByExpression in specification.OrderByExpressions)
            {
                if (orderByExpression.Equals(firstOrderByExpression))
                {
                    continue;
                }

                orderedQuery = orderByExpression.OrderByDirection.Equals(OrderByDirection.Ascending)
                    ? orderedQuery.ThenBy(orderByExpression.Expression)
                    : orderedQuery.ThenByDescending(orderByExpression.Expression);
            }

            return orderedQuery;
        }

        private static IQueryable<TEntity> SetPaging(IQueryable<TEntity> outputQuery, ISpecification<TEntity> specification)
        {
            if (!specification.IsPagingEnabled)
            {
                return outputQuery;
            }

            return outputQuery.Skip(specification.Skip).Take(specification.Take);
        }

        private static IQueryable<TEntity> SetTracking(IQueryable<TEntity> outputQuery, ISpecification<TEntity> specification)
        {
            if (specification.AsTracking)
            {
                return outputQuery;
            }

            return outputQuery.AsNoTracking();
        }
    }
}
