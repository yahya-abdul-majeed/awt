namespace ibex.Models
{
    public class Bug
    {
        public int id { get; set; }
        public string name { get; set; }    
        public int website_id { get; set; }
        public string description { get; set; }
        public string commit_link { get; set; }
        public string author { get; set; }
    }
}
