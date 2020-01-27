using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace my_time_api.Model
{
    public class Task
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PlaceId { get; set; }
        public string UserId { get; set; }

    }
}