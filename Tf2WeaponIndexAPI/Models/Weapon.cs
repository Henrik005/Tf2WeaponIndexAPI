using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Tf2WeaponIndexAPI.Models
{
    public class Weapon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("weapon_name")]
        public string WeaponName { get; set; } = null!;
        [BsonElement("stats")]
        public string Stats { get; set; } = null!;
        [BsonElement("img")]
        public string Image { get; set; } = null!;
        [BsonElement("class")]
        public string Class { get; set; } = null!;
        [BsonElement("description")]
        public string? Description { get; set; } = null!;

    }
}
