using Confluent.Kafka;
using Kafka_Test_Project.Models;

namespace Kafka_Test_Project.Serialization
{
    internal class MessagePackDeserializer<T> : IDeserializer<ChatMessage>
    {
        public ChatMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            throw new NotImplementedException();
        }
    }
}