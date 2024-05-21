namespace lynx.Models.DTO
{
    public class GetContestDTO
    {
        public int id { get; set; }
        public string name { get; set; }    
        public string description { get; set; }
        public bool is_active { get;set; }
        public DateTime opening_time { get; set; }  
        public DateTime closing_time { get;set; }

        public IEnumerable<Assignment> Assignments { get; set; } =  new List<Assignment>();
    }
}
