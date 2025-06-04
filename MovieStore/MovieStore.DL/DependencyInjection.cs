using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.DL.Cache;
using MovieStore.DL.Gateways;
using MovieStore.DL.Interfaces;
using MovieStore.DL.Kafka;
using MovieStore.DL.Kafka.KafkaCache;
using MovieStore.DL.Kafka.KafkaInterface;
using MovieStore.DL.Repositories;
using MovieStore.Models.Configurations.CachePopulator;
using MovieStore.Models.DTO;
using MovieStoreB.DL.Cache;

namespace MovieStoreB.DL
{
    public static class DependencyInjection
    {
        public static IServiceCollection
            AddDataDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IMovieRepository, MovieRepository>();
            services.AddSingleton<IActorRepository, ActorRepository>();
            services.AddSingleton<IActorBioGateway, ActorBioGateway>();

            services.AddCache<MoviesCacheConfiguration, MovieRepository, Movie, string>(config);
            services.AddCache<ActorsCacheConfiguration, ActorRepository, Actor, string>(config);

            services.AddSingleton<IKafkaCache<string, Movie>, KafkaCacheConsumer<string, Movie>>();
            services.AddHostedService(sp => (KafkaCacheConsumer<string, Movie>)sp.GetRequiredService<IKafkaCache<string, Movie>>());

            //services.AddSingleton<IKafkaCache<string, Actor>, KafkaCacheConsumer<string, Actor>>();
            //services.AddHostedService(sp => (KafkaCacheConsumer<string, Actor>)sp.GetRequiredService<IKafkaCache<string, Actor>>());


            return services;
        }

        public static IServiceCollection AddCache<TCacheConfiguration, TCacheRepository, TData, TKey>(this IServiceCollection services, IConfiguration config)
           where TCacheConfiguration : CacheConfiguration
           where TCacheRepository : class, ICacheRepository<TKey, TData>
           where TData : ICacheItem<TKey>
           where TKey : notnull
        {
            var configSection = config.GetSection(typeof(TCacheConfiguration).Name);

            if (!configSection.Exists())
            {
                throw new ArgumentNullException(typeof(TCacheConfiguration).Name, "Configuration section is missing in appsettings!");
            }

            services.Configure<TCacheConfiguration>(configSection);

            services.AddSingleton<ICacheRepository<TKey, TData>, TCacheRepository>();
            services.AddSingleton<IKafkaProducer<TData>, KafkaProducer<TKey, TData, TCacheConfiguration>>();
            services.AddHostedService<MongoCachePopulator<TData, TCacheConfiguration, TKey>>();


            return services;
        }
    }


}
