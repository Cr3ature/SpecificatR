using Microsoft.Extensions.DependencyInjection;
using SpecificatR.Infrastructure.Repositories;

namespace SpecificatR.Infrastructure.Configuration
{
    public static class SpecificatRServiceCollectionExtensions
    {
        public static IServiceCollection AddSpecificatR(this IServiceCollection services)
        {
            services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
            services.AddScoped(typeof(IReadWriteRepository<,>), typeof(ReadWriteRepository<,>));

            return services;
        }
    }
}
