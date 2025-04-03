using Confluent.Kafka;

namespace KafkaTest_Consumer
{
    public class MsgPackerSerializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePack.MessagePackSerializer.Deserialize<T>(data.ToArray());
        }
    }
}
