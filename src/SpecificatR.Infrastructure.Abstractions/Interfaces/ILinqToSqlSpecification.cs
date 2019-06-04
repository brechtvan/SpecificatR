// <copyright file="ILinqToSqlSpecification.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Abstractions.Interfaces
{
    public interface ILinqToSqlSpecification
    {
        bool IsPagingEnabled { get; }

        int Skip { get; }

        int Take { get; }
    }
}
