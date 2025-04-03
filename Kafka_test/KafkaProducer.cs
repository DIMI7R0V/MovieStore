using Confluent.Kafka;
using Kafka_Test_Project.Models;
using MessagePack;

namespace Kafka_Test_Project
{
    public class KafkaProducer
    {
        private readonly IProducer<Null, byte[]> _producer;

        public KafkaProducer()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "kafka-193981-0.cloudclusters.net:10300",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "admin",
                SaslPassword = "CPxpKSRD",
                EnableSslCertificateVerification = false
            };

            _producer = new ProducerBuilder<Null, byte[]>(config).Build();
        }

        public async Task SendMessage(ChatMessage message)
        {
            var bytes = MessagePackSerializer.Serialize(message);
            await _producer.ProduceAsync("test", new Message<Null, byte[]> { Value = bytes });
        }
    }
}
