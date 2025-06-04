namespace MovieStore.DL.Kafka.KafkaCache
{
    public interface IKafkaCache<TKey, TValue>
        where TKey : notnull
        where TValue : class
    {
    }
}
