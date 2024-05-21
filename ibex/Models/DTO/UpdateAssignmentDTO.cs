namespace ibex.Models.DTO
{
    public class UpdateAssignmentDTO
    {
        public int id { get; set; }
        public int points { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string author { get; set; } = string.Empty;
        public List<WebsiteItem> websites { get; set; } = new List<WebsiteItem>();
        public List<int> skeleton_ids { get; set; } = new List<int>();
    }
}
