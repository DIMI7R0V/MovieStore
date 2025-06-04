using Confluent.Kafka;
using MessagePack;
using MessagePack.Resolvers;

namespace MovieStore.Models.Serialization
{
    public class MessagePackDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<T>(data.ToArray(),
            MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance));
        }
    }
}
