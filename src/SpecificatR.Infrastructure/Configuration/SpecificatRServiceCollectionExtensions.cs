//-----------------------------------------------------------------------
// <copyright file="SpecificatRServiceCollectionExtensions.cs" company="David Vanderheyden">
// Copyright (c) David Vanderheyden. All rights reserved.
// Licensed under the Apache-2.0 license. See https://licenses.nuget.org/Apache-2.0 for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SpecificatR.Infrastructure.Repositories;

    public static class SpecificatRServiceCollectionExtensions
    {
        public static IServiceCollection AddSpecificatR<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.AddScoped(typeof(IReadRepository<,,>), typeof(ReadRepository<,,>));
            services.AddScoped(typeof(IReadWriteRepository<,,>), typeof(ReadWriteRepository<,,>));

            return services;
        }
    }
}
