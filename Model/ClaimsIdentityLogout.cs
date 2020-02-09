using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace my_time_api.Model
{
    public class ClaimsIdentityLogout
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClaimsIdentityLogoutId { get; set; }
        public string Name { get; set; }
        public string NameIdentifier { get; set; }
        public string Role { get; set; }
    }
}