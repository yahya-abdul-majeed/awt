namespace lynx.Models
{
    public class Assignment
    {
        public int id { get; set; }
        public int parent_id { get; set; }
        public int points { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string website_link { get; set; } = string.Empty;

    }
}
