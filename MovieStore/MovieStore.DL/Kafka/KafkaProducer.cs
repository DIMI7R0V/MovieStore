using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MovieStore.DL.Kafka.KafkaInterface;
using MovieStore.Models.Configurations.CachePopulator;
using MovieStore.Models.DTO;
using MovieStore.Models.Serialization;

namespace MovieStore.DL.Kafka
{
    internal class KafkaProducer<TKey, TData, TConfiguration> : IKafkaProducer<TData> 
        where TData : ICacheItem<TKey> 
        where TKey : notnull
        where TConfiguration : CacheConfiguration
    {
        private readonly ProducerConfig _config;
        private readonly IProducer<TKey, TData> _producer;
        private readonly IOptionsMonitor<TConfiguration> _kafkaConfig;
        public KafkaProducer(IOptionsMonitor<TConfiguration> kafkaConfig)
        {
            _config = new ProducerConfig()
            {
                BootstrapServers = "pkc-w7d6j.germanywestcentral.azure.confluent.cloud:9092",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "D5QN7ABI64XMHO6B",
                SaslPassword = "c2LzQ9HAhrCext2rkulT6m9n+xsxuxTUCUPbds0wxNXttNljf3UlLh2f/4YX0x27",
                EnableSslCertificateVerification = false
            };

            _producer = new ProducerBuilder<TKey, TData>(_config)
                .SetValueSerializer(new MsgPackSerializer<TData>())
                .Build();
            _kafkaConfig = kafkaConfig;
        }

        public async Task Produce(TData message)
        {
            await _producer.ProduceAsync(_kafkaConfig.CurrentValue.Topic, new Message<TKey, TData>
            {
                Key = message.GetKey(),
                Value = message
            });
        }

        public async Task ProduceAll(IEnumerable<TData> messages)
        {
            //var tasks = messages.Select(message => Produce(message));

            //await Task.WhenAll(tasks);

            await ProduceBatches(messages);
        }

        public async Task ProduceBatches(IEnumerable<TData> messages)
        {
            const int batchSize = 50;
            var batch = new List<Task>();

            foreach (var message in messages)
            {
                batch.Add(Produce(message));

                if (batch.Count == batchSize)
                {
                    await Task.WhenAll(batch);
                    batch.Clear();
                }
            }

            // Process any remaining messages
            if (batch.Count > 0)
            {
                await Task.WhenAll(batch);
            }
        }
    }
}
