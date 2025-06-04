using MovieStore.BL;
using MovieStore.Models.Configurations;

namespace MovieStore.ServiceExtentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MongoDbConfiguration>(config.GetSection(nameof(MongoDbConfiguration)));

            return services;
        }
    }
}
