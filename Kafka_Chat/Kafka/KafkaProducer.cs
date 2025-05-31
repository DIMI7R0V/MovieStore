using Confluent.Kafka;
using Kafka_Test_Project.Models;
using Kafka_Test_Project.Serialization;
using MessagePack;

namespace Kafka_Test_Project.Serialization
{
    public class KafkaProducer
    {
        private readonly ProducerConfig _config;
        private readonly IProducer<string, ChatMessage> _producer;

        public KafkaProducer()
        {
            _config = new ProducerConfig()
            {
                BootstrapServers = "kafka-193981-0.cloudclusters.net:10300",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "admin",
                SaslPassword = "CPxpKSRD",
                EnableSslCertificateVerification = false
            };

            _producer = new ProducerBuilder<string, ChatMessage>(_config)
                .SetValueSerializer(new MsgPackSerializer<ChatMessage>())
                .Build();
        }

        public void Producer(ChatMessage message)
        {
            _producer.Produce("test", new Message<string, ChatMessage>
            {
                Key = message.Id,
                Value = message
            });
        }

    }
}
