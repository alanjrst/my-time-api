using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace my_time_api.Model
{
    public class Place
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PlaceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int range { get; set; }
        public string UserId { get; set; }
    }
}