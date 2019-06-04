// <copyright file="ISpecification.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface ISpecification<TClass>
    {
        bool AsTracking { get; }

        Expression<Func<TClass, bool>> Criteria { get; }

        bool IgnoreQueryFilters { get; }

        IReadOnlyCollection<Expression<Func<TClass, object>>> Includes { get; }

        bool IsPagingEnabled { get; }

        IReadOnlyCollection<OrderByExpression<TClass>> OrderByExpressions { get; }

        int Skip { get; }

        int Take { get; }
    }
}
