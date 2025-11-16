namespace Tf2WeaponIndexAPI.Models
{
    public class Tf2Item
    {
        public string name { get; set; }
        public string item_type_name { get; set; }
        public string item_class { get; set; }
        public int min_ilevel { get; set; }
        public List<Tf2ItemAttribute>? attributes { get; set; }
        public string image_url_large { get; set; }
        public List<string> used_by_classes { get; set; }
    }
}
