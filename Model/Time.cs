using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace my_time_api.Model
{
    public class Time
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TimeId { get; set; }
        public string Name { get; set; }
        public DateTime DateInital { get; set; }
        public DateTime DateEnd { get; set; }
        public string TaskId { get; set; }
        public string PlaceId { get; set; }
    }
}