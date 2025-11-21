using System.Text.Json.Serialization;
namespace Tf2WeaponIndexAPI.Models
{
    public class Tf2ItemAttribute
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }
    }
}
