using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tf2WeaponIndexAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [BsonElement("email")]
        public string? Email { get; set; } = null!;
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = null!;
    }
}
