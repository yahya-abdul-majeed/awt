namespace ibex.Models.DTO
{
    public class UpdateWebsiteDTO
    {
        public int id { get; set; } 
        public int? parent_id { get; set; }
        public string hosted_at { get; set; }
        public string repository { get; set; }
        public string description { get; set; }
        public List<BugDTO> bugs { get; set; } = new List<BugDTO>();
    }
}
