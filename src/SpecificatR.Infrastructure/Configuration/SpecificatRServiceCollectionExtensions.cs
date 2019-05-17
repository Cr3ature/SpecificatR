using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpecificatR.Infrastructure.Internal;
using SpecificatR.Infrastructure.Repositories;

namespace SpecificatR.Infrastructure.Configuration
{
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
