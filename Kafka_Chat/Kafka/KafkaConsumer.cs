using Confluent.Kafka;
using Kafka_Test_Project.Models;
using Kafka_Test_Project.Serialization;


namespace Kafka_Test_Project.Serialization
{
    public class KafkaConsumer
    {
        public static void Consume()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "kafka-193981-0.cloudclusters.net:10300",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "admin",
                SaslPassword = "CPxpKSRD",
                EnableSslCertificateVerification = false,
                GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Latest
            };


            using (var consumer = new ConsumerBuilder<string, ChatMessage>(config)
                .SetValueDeserializer(new MessagePackDeserializer<ChatMessage>())
                .Build())
            {
                consumer.Subscribe("test");
                while (true)
                {
                    var consumerResult = consumer.Consume();

                    ConsoleHelper.WriteMsgFromUser(consumerResult.Message.Value);
                    ConsoleHelper.WriteLocalUser();
                }

            }

            public void Start() => Task.Run(() => Consume());
        }
    }
}
