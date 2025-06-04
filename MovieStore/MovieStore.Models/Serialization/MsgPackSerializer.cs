using Confluent.Kafka;
using MessagePack;
using MessagePack.Resolvers;

namespace MovieStore.Models.Serialization
{
    public class MsgPackSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return MessagePackSerializer.Serialize(data,
            MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance));
        }
    }
}
