using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MovieStore.DL.Cache;
using MovieStore.DL.Kafka.KafkaInterface;
using MovieStore.Models.Configurations.CachePopulator;
using MovieStore.Models.DTO;

namespace MovieStoreB.DL.Cache
{
    public class MongoCachePopulator<TData, TConfigurationType, TKey> : BackgroundService
        //where TDataRepository : ICacheRepository<TKey, TData>
        where TKey : notnull
        where TData : ICacheItem<TKey>
        where TConfigurationType : CacheConfiguration
    {
        private readonly ICacheRepository<TKey, TData> _cacheRepository;
        private readonly IOptionsMonitor<TConfigurationType> _configuration;
        private readonly IKafkaProducer<TData> _kafkaProducer;

        public MongoCachePopulator(
            ICacheRepository<TKey, TData> cacheRepository, 
            IOptionsMonitor<TConfigurationType> configuration, 
            IKafkaProducer<TData> kafkaProducer)
        {
            _cacheRepository = cacheRepository;
            _configuration = configuration;
            _kafkaProducer = kafkaProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastExecuted = DateTime.UtcNow;

            var result = await _cacheRepository.FullLoad();

            if (result != null && result.Any())
            {
                await _kafkaProducer.ProduceAll(result);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(_configuration.CurrentValue.RefreshInterval), stoppingToken);

                var updatedData = await _cacheRepository.DifLoad(lastExecuted);

                if (updatedData != null && updatedData.Any(d => d.DateInserted > lastExecuted))
                {
                    await _kafkaProducer.ProduceAll(updatedData);
                    Console.WriteLine(updatedData.FirstOrDefault()?.DateInserted);

                    lastExecuted = updatedData.Max(d => d.DateInserted);
                }
            }
        }
    }
}
