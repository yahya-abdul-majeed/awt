namespace lynx.Models.DTO
{
    public class AddGroupsToContestDTO
    {
        public int contest_id { get; set; }
        public string[] groupids { get; set; }
    }
}
