using MongoDB.Driver;
using my_time_api.Model;
using System.Collections.Generic;
using System.Linq;

namespace my_time_api.Services
{
    public class MyTimeService
    {
        public readonly IMongoCollection<Place> _place;
        public readonly FindOptions<Place> _optionsPlace;
        public readonly IMongoCollection<User> _user;

        public MyTimeService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _place = database.GetCollection<Place>(settings.PlaceCollectionName);
            _user = database.GetCollection<User>(settings.UserCollectionName);

            _optionsPlace = new FindOptions<Place>{ Sort = Builders<Place>.Sort.Ascending(place => place.PlaceId) };            
        }        
    }
}