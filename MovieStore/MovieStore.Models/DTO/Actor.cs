﻿using MessagePack;

namespace MovieStore.Models.DTO
{
    [MessagePackObject]
    public class Actor : ICacheItem<string>
    {
        [Key(0)]
        public string Id { get; set; } 

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public string Bio { get; set; } 

        [Key(3)]
        public DateTime DateInserted { get; set; }

        public string GetKey()
        {
            return Id;
        }
    }
}