namespace ibex.Models
{
    public class Website
    {
        public int id { get; set; }
        public bool should_pass { get; set; }
        public int? parent_id { get; set; }  
        public string repository { get; set; }
        public string hosted_at { get; set; }
        public string description { get; set; }
    }
}