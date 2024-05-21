namespace lynx.Models
{
    public class Submission
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int assignment_id { get; set; }
        public int assignment_parent_id { get;set; }
        public int language_id { get; set; }
        public int test_fw_id { get; set; }
        public string lang_fw { get; set; } = string.Empty;
        public string source_code { get; set; } = string.Empty;
        public byte[]? packages { get; set; } = null;
        public byte[]? result { get; set; } = null;
        public Status status { get; set; }
        public Verdict auto_verdict { get; set; }
        public Verdict? postmoderation_verdict { get; set; } = null;
        public DateTime created_at { get; set; }
        public DateTime finished_at { get; set; }
        public string execution_host { get; set; } = string.Empty;
        public string time { get; set; } = string.Empty;
        public string time_wall { get; set; } = string.Empty;
        public string max_rss { get; set; } = string.Empty;
        public string memory { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public int exitcode { get; set; }


    }
    //public class Submission
    //{
    //    public int id { get; set; }
    //    public int user_id { get; set; }
    //    public int assignment_id { get; set; }
    //    public int assignment_parent_id { get;set; }
    //    public int language_id { get; set; }
    //    public int test_fw_id { get; set; }
    //    public string source_code { get; set; } = string.Empty;
    //    public byte[]? packages { get; set; } = null;
    //    public Status status { get; set; }
    //    public Verdict auto_verdict { get; set; }
    //    public Verdict? postmoderation_verdict { get; set; } = null;
    //    public DateTime created_at { get; set; }
    //    public DateTime finished_at { get; set; }
    //    public string compilation_standard_output { get; set; } = string.Empty;
    //    public string compilation_standard_error { get; set; } = string.Empty;
    //    public string run_standard_output { get; set; } = string.Empty;
    //    public string run_standard_error { get; set; } = string.Empty;
    //    public string packageinstallation_standard_output { get; set; } = string.Empty;
    //    public string packageinstallatoin_standard_error { get; set; } = string.Empty;
    //}

    public enum Status
    {
        INQUEUE,
        PROCESSING,
        COMPLETED,
        COMPILATIONERROR,
        RE,
        SG,
        TO,
        XX
    }

    public enum Verdict
    {
        ACCEPTED,
        REJECTED,
        PENDING
    }
}
