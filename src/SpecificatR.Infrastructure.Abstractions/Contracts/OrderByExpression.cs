// <copyright file="OrderByExpression.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Abstractions
{
    using System;
    using System.Linq.Expressions;

    public class OrderByExpression<T>
    {
        public OrderByExpression(Expression<Func<T, object>> expression, OrderByDirection orderByDirection)
        {
            Expression = expression;
            OrderByDirection = orderByDirection;
        }

        public Expression<Func<T, object>> Expression { get; set; }

        public OrderByDirection OrderByDirection { get; set; }
    }
}
