using System.Text.Json;
using System.Text;
using System.ComponentModel;
using Tf2WeaponIndexAPI.Models;
using System.Net.WebSockets;

namespace Tf2WeaponIndexAPI.Services
{

    public class DataMapper
    {
        public readonly Dictionary<string, Tf2AttributeDefinition> attributeDefs;
        public readonly Dictionary<string, Tf2Item> TfItem;
        public DataMapper()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var rawAttributes = JsonSerializer.Deserialize<Dictionary<string, Tf2AttributeDefinition>>(
                File.ReadAllText("Tf2ItemAttributes.json"));
            attributeDefs = rawAttributes.Values.ToDictionary(attribute => attribute.Name, attribute => attribute);

            var rawItems = JsonSerializer.Deserialize<List<Tf2Item>>(
                File.ReadAllText("tf2_weapons.json"));
            TfItem = rawItems.ToDictionary(item => item.name, item => item);
            var e = 0;




        }

        public List<string> FormatItem(List<Tf2Item> items)
        {
            var formattedItems = new List<string>();
           
            foreach (var item in items)
            {
                var sb = new StringBuilder();
                sb.AppendLine(item.name);
                sb.AppendLine($"Level {item.min_ilevel} {item.item_type_name.Replace("#TF_Weapon_", "")}");
                if (item.attributes != null)
                {


                    foreach (var attr in item.attributes)
                    {
                        if (attr.Class != null && attributeDefs.TryGetValue(attr.Class, out var def))
                        {
                            sb.AppendLine(FormatAttribute(attr, def));
                        }
                        else
                        {
                            sb.AppendLine(attr.Name);
                        }
                       
                    }
                }
                else
                {
                    sb.AppendLine("Default Weapon");
                }
                formattedItems.Add(sb.ToString());
            }
            return formattedItems;
        }


        public string FormatAttribute(Tf2ItemAttribute attr, Tf2AttributeDefinition def)
        {
            if (def == null) return attr.Name;
            string formattedValue;
            switch (def.ValueType.ToLowerInvariant())
            {
                case "percentage":
                    double percent = (attr.Value - 1) * 100;
                    formattedValue = $"{(percent > 0 ? "+" : "")}{percent:0.#}";
                    break;
                case "additive":
                    formattedValue = $"{(attr.Value > 0 ? "+" : "")}{attr.Value:0.#}";
                    break;
                case "integer":
                    formattedValue = $"{attr.Value:0}";
                    break;
                case "boolean":
                default:
                    formattedValue = "";
                    break;
            }
            string text = def.Description;
            text = text.Replace("%s1%", formattedValue);
            return text.Trim();
        }

    }

}
