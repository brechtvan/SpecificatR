﻿//-----------------------------------------------------------------------
// <copyright file="SpecificatRServiceCollectionExtensions.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:43</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SpecificatR.Infrastructure.Repositories;

    /// <summary>
    /// Defines the <see cref="SpecificatRServiceCollectionExtensions" />
    /// </summary>
    public static class SpecificatRServiceCollectionExtensions
    {
        /// <summary>
        /// The AddSpecificatR
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services">The services<see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddSpecificatR<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.AddScoped(typeof(IReadRepository<,,>), typeof(ReadRepository<,,>));
            services.AddScoped(typeof(IReadWriteRepository<,,>), typeof(ReadWriteRepository<,,>));

            return services;
        }
    }
}
