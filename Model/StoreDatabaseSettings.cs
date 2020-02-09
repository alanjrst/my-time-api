namespace my_time_api.Model
{
    public class StoreDatabaseSettings : IStoreDatabaseSettings
    {
        public string PlaceCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string JobCollectionName { get; set; }
        public string TaskCollectionName { get; set; }
        public string TimeCollectionName { get; set; }
        public string ClaimsIdentityLogoutCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IStoreDatabaseSettings
    {
        string PlaceCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string JobCollectionName { get; set; }
        string TimeCollectionName { get; set; }
        string ClaimsIdentityLogoutCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}