using System.Runtime.InteropServices;

namespace ibex.Models.DTO
{
    public class WebsiteNodeDTO
    {
        public int id { get; set; }
        public int parent_id { get; set; }  
        public string repository { get; set; }
        public string hosted_at { get; set; }
        public string description { get; set; }
        public List<WebsiteNodeDTO> children { get; set; } = new List<WebsiteNodeDTO>();
    }
}
