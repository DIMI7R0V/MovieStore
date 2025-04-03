using Confluent.Kafka;
using MessagePack;

namespace KafkaTest_Consumer
{
    public class MsgPackservSerialise<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {

            return MessagePackSerializer.Serialize(data);
        }
    }
}
