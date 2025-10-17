using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tf2WeaponIndexAPI.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
