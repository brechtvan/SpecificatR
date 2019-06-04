// <copyright file="IBaseEntity.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>

namespace SpecificatR.Infrastructure.Abstractions
{
    public interface IBaseEntity<TIdentifier>
    {
        TIdentifier Id { get; set; }
    }
}
