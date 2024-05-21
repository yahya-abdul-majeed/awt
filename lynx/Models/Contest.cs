namespace lynx.Models
{
    public class Contest
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty; 
        public string description { get; set; } = string.Empty;
        public bool is_active { get;set; }
        public DateTime opening_time { get; set; }  
        public DateTime closing_time { get;set; }
        public List<Assignment> assignments { get; set; } = new();
    }
}
