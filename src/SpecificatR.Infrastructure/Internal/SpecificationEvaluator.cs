﻿//-----------------------------------------------------------------------
// <copyright file="SpecificationEvaluator.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:45</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Internal
{
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="SpecificationEvaluator{ClassType, IdType}" />
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    internal class SpecificationEvaluator<TEntity, TIdentifier>
        where TEntity : class, IBaseEntity<TIdentifier>
    {
        /// <summary>
        /// The GetQuery
        /// </summary>
        /// <param name="inputQuery">The inputQuery<see cref="IQueryable{ClassType}"/></param>
        /// <param name="specification">The specification<see cref="ISpecification{ClassType}"/></param>
        /// <returns>The <see cref="IQueryable{ClassType}"/></returns>
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

        /// <summary>
        /// The GetQueryWithCount
        /// </summary>
        /// <param name="inputQuery">The inputQuery<see cref="IQueryable{ClassType}"/></param>
        /// <param name="specification">The specification<see cref="ISpecification{ClassType}"/></param>
        /// <returns>The <see cref="(IQueryable{TEntity} query, int filteredCount)"/></returns>
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

        /// <summary>
        /// The SetCriteria
        /// </summary>
        /// <param name="outputQuery">The outputQuery<see cref="IQueryable{ClassType}"/></param>
        /// <param name="criteria">The criteria<see cref="Expression{Func{TEntity, bool}}"/></param>
        /// <returns>The <see cref="IQueryable{ClassType}"/></returns>
        private static IQueryable<TEntity> SetCriteria(IQueryable<TEntity> outputQuery, Expression<Func<TEntity, bool>> criteria)
        {
            if (criteria == null)
            {
                return outputQuery;
            }

            return outputQuery.Where(criteria);
        }

        /// <summary>
        /// The SetIgnoreQueryFilters
        /// </summary>
        /// <param name="outputQuery">The outputQuery<see cref="IQueryable{ClassType}"/></param>
        /// <param name="specification">The specification<see cref="ISpecification{ClassType}"/></param>
        /// <returns>The <see cref="IQueryable{ClassType}"/></returns>
        private static IQueryable<TEntity> SetIgnoreQueryFilters(IQueryable<TEntity> outputQuery, ISpecification<TEntity> specification)
        {
            if (specification.IgnoreQueryFilters)
            {
                return outputQuery.IgnoreQueryFilters();
            }

            return outputQuery;
        }

        /// <summary>
        /// The SetIncludes
        /// </summary>
        /// <param name="outputQuery">The outputQuery<see cref="IQueryable{ClassType}"/></param>
        /// <param name="specification">The specification<see cref="ISpecification{ClassType}"/></param>
        /// <returns>The <see cref="IQueryable{ClassType}"/></returns>
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

        /// <summary>
        /// The SetOrderBy
        /// </summary>
        /// <param name="outputQuery">The outputQuery<see cref="IQueryable{ClassType}"/></param>
        /// <param name="specification">The specification<see cref="ISpecification{ClassType}"/></param>
        /// <returns>The <see cref="IQueryable{ClassType}"/></returns>
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

        /// <summary>
        /// The SetPaging
        /// </summary>
        /// <param name="outputQuery">The outputQuery<see cref="IQueryable{ClassType}"/></param>
        /// <param name="specification">The specification<see cref="ISpecification{ClassType}"/></param>
        /// <returns>The <see cref="IQueryable{ClassType}"/></returns>
        private static IQueryable<TEntity> SetPaging(IQueryable<TEntity> outputQuery, ISpecification<TEntity> specification)
        {
            if (!specification.IsPagingEnabled)
            {
                return outputQuery;
            }

            return outputQuery.Skip(specification.Skip).Take(specification.Take);
        }

        /// <summary>
        /// The SetTracking
        /// </summary>
        /// <param name="outputQuery">The outputQuery<see cref="IQueryable{ClassType}"/></param>
        /// <param name="specification">The specification<see cref="ISpecification{ClassType}"/></param>
        /// <returns>The <see cref="IQueryable{ClassType}"/></returns>
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
