using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using MovieStore.DL.Kafka.KafkaCache;
using MovieStore.Models.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.DL.Kafka
{
    public class KafkaCacheConsumer<TKey, TValue> : BackgroundService, IKafkaCache<TKey, TValue>
        where TKey : notnull
        where TValue : class
    {
        private readonly ConsumerConfig _config;
        private readonly ConcurrentDictionary<TKey, TValue> _cache = new();

        public KafkaCacheConsumer()
        {
            _config = new ConsumerConfig
            {
                BootstrapServers = "pkc-w7d6j.germanywestcentral.azure.confluent.cloud:9092",
                GroupId = $"KafkaChat-{Guid.NewGuid}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "D5QN7ABI64XMHO6B",
                SaslPassword = "c2LzQ9HAhrCext2rkulT6m9n+xsxuxTUCUPbds0wxNXttNljf3UlLh2f/4YX0x27",
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                using var consumer = new ConsumerBuilder<TKey, TValue>(_config)
                    .SetValueDeserializer(new MessagePackDeserializer<TValue>())
                    .Build();

                consumer.Subscribe("movie_cache");

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var message = consumer.Consume(stoppingToken);
                        if (message?.Message != null)
                        {
                            Console.WriteLine($"Consumed: {message.Message.Key}");
                            _cache[message.Message.Key] = message.Message.Value;

                            Console.WriteLine($"Cache size now: {_cache.Count}");

                            foreach (var kvp in _cache)
                            {
                                Console.WriteLine($"Cache: {kvp.Key}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Consumer failed: {ex.Message}");
                }
            }, stoppingToken);
        }
        public TValue? GetByKey(TKey key)
        {
            _cache.TryGetValue(key, out var value);
            return value;
        }

        public IReadOnlyDictionary<TKey, TValue> GetAll()
        {
            return _cache;
        }
    }
}
