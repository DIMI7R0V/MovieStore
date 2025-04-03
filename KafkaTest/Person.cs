using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace KafkaTest_Consumer
{
    [MessagePackObject]
    public class Person
    {
        [Key(0)]
        public Guid Id { get; set; }

        [Key(1)]
        public string Name { get; set; }
    }
}
