namespace Tf2WeaponIndexAPI.Models
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string WeaponCollectionName { get; set; } = null!;
        public string PostCollectionName { get; set; } = null!;
    }
}
