namespace lynx.Models.DTO
{
    public class SubmissionDTO
    {
        public int user_id { get; set; }
        public int assignment_id { get; set; }
        public int assignment_parent_id { get;set; }
        public int language_id { get; set; }
        public int test_fw_id { get; set; }
        public string source_code { get; set; } = string.Empty;
    }
}
