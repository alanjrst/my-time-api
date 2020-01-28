using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace my_time_api.Model
{
    public class Job
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string JobId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PlaceId { get; set; }
        public string UserId { get; set; }

    }
}