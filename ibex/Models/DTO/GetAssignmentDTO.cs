namespace ibex.Models.DTO
{
    public class GetAssignmentDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int points { get; set; }
        public string website_link { get; set; }

        //additions
        public string author { get; set; }
    }
}
