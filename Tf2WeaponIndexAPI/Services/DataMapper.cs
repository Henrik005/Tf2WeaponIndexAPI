using System.Text.Json;
using System.Text;
using System.Text.Json;
using System.ComponentModel;
using Tf2WeaponIndexAPI.Models;
using System.Net.WebSockets;

namespace Tf2WeaponIndexAPI.Services
{

    public class DataMapper
    {
        private readonly Dictionary<string, Tf2AttributeDefinition> attributeDefs;
        public DataMapper()
        {
            var raw = JsonSerializer.Deserialize<Dictionary<string, Tf2AttributeDefinition>>(
                File.ReadAllText("Tf2ItemAttributes.json"));
            var attributeDefs = raw.Values.ToDictionary(attribute => attribute.Class, attribute => attribute);

        }

        void FormatItem(List<Tf2Item> items)
        {

            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.AppendLine(item.Name);
                sb.AppendLine($"Level {item.MinILevel} {item.ItemTypeName.Replace("#TF_Weapon_", "")}");
                foreach(var attr in item.Attributes)
                {
                    if (attributeDefs.TryGetValue(attr.Class, out var def))
                    {
                        sb.AppendLine(FormatAttribute(attr, def));
                    }
                    else
                    {
                        sb.AppendLine(attr.Name);
                    }
                    sb.AppendLine("\n");
                }
            }
        }


        string FormatAttribute(Tf2ItemAttribute attr, Tf2AttributeDefinition def)
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
