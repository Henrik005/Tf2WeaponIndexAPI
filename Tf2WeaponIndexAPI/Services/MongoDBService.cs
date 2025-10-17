using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tf2WeaponIndexAPI.Models;

namespace Tf2WeaponIndexAPI.Services
{
    public class MongoDBService
    {
        public IMongoCollection<User> Users { get; }
        public IMongoCollection<Weapon> Weapons { get; }
        public IMongoCollection<Post> Posts { get; }

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var settings = mongoDBSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Users = database.GetCollection<User>(settings.UserCollectionName);
            Weapons = database.GetCollection<Weapon>(settings.WeaponCollectionName);
            Posts = database.GetCollection<Post>(settings.PostCollectionName);
        }
        public async Task CreateUser(User newUser)
        {
            await Users.InsertOneAsync(newUser);
        }

        

    }
}
