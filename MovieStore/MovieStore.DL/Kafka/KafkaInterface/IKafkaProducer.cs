namespace MovieStore.DL.Kafka.KafkaInterface
{
    public interface IKafkaProducer<TData>
    {
        Task ProduceAll(IEnumerable<TData> messages);

        Task Produce(TData message);

        Task ProduceBatches(IEnumerable<TData> messages);
    }
}
