using Confluent.Kafka;

namespace KafkaTest_Consumer
{
    internal class Program
    {
        static IProducer<string, Person> CreateProducer()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            return new ProducerBuilder<string, Person>(config)
                .SetValueSerializer(new MsgPackservSerialise<Person>())
                .Build();
        }

        static async Task ProduceAsync(CancellationToken token)
        {
            Console.WriteLine(" Producer starting...");
            var producer = CreateProducer();
            Console.WriteLine("Producer created.");

            while (!token.IsCancellationRequested)
            {
                var key = Guid.NewGuid();
                var person = new Person
                {
                    Id = key,
                    Name = "John Doe"
                };

                var msg = new Message<string, Person>
                {
                    Key = key.ToString(),
                    Value = person
                };

                try
                {
                    var result = await producer.ProduceAsync("persons", msg, token);
                    Console.WriteLine($" Sent: {person.Id} - {person.Name} to {result.TopicPartitionOffset}");
                }
                catch (ProduceException<string, Person> ex)
                {
                    Console.WriteLine($"Produce failed: {ex.Error.Reason}");
                }

                await Task.Delay(1000, token);
            }
        }

        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();

            // Run both Producer and Consumer as tasks in parallel
            var producerTask = ProduceAsync(cts.Token);
            var consumerTask = ConsumerProgram.StartConsumerAsync(cts.Token);

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Cancellation requested...");
            };

            await Task.WhenAll(producerTask, consumerTask);
        }
    }

    internal class ConsumerProgram
    {
        public static Task StartConsumerAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = "localhost:9092",
                    GroupId = "test-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using var consumer = new ConsumerBuilder<string, Person>(config)
                    .SetValueDeserializer(new MsgPackerSerializer<Person>())
                    .Build();

                consumer.Subscribe("persons");
                Console.WriteLine("Consumer started. Listening...");

                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        try
                        {
                            var result = consumer.Consume(token);

                            if (result?.Message?.Value != null)
                            {
                                Console.WriteLine($"Received: {result.Message.Value.Name} ({result.Message.Value.Id})");
                            }
                        }
                        catch (ConsumeException ex)
                        {
                            Console.WriteLine($"Consume error: {ex.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Consumer shutting down...");
                }
                finally
                {
                    consumer.Close();
                    Console.WriteLine("Consumer closed.");
                }
            });
        }
    }
}
