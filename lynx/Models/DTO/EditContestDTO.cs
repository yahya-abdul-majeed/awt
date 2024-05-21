namespace lynx.Models.DTO
{
    public class EditContestDTO
    {
        public string[] groups { get; set; }
        public string[] assignments { get; set; }
        public Contest contest { get; set; }
    }
}
