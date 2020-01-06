//-----------------------------------------------------------------------
// <copyright file="SpecificatRServiceCollectionExtensions.cs">
//     Copyright (c) 2019-2020 David Vanderheyden All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SpecificatR;

    /// <summary>
    /// Defines the <see cref="SpecificatRServiceCollectionExtensions"/>.
    /// </summary>
    public static class SpecificatRServiceCollectionExtensions
    {
        /// <summary>
        /// Service collection extension to add the specificatR setup registery to the DI container.
        /// </summary>
        /// <typeparam name="TDbContext">The dbcontext <see cref="TDbContext"/>.</typeparam>
        /// <param name="services">The services <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSpecificatR<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.AddScoped(typeof(IReadCoreRepository<,>), typeof(ReadCoreRepository<,>));
            services.AddScoped(typeof(IReadWriteCoreRepository<,>), typeof(ReadWriteCoreRepository<,>));

            services.AddScoped(typeof(IReadBaseRepository<,,>), typeof(ReadBaseRepository<,,>));
            services.AddScoped(typeof(IReadWriteBaseRepository<,,>), typeof(ReadWriteBaseRepository<,,>));

            return services;
        }
    }
}
