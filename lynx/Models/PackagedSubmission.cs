namespace lynx.Models
{
    public class PackagedSubmission
    {
        public PackagedSubmission(string sourcecode, int lang_id, SubmissionContext context)
        {
            this.context = context;
            this.source_code = sourcecode;
            this.language_id = lang_id;
        }
        public string source_code { get; set; } = string.Empty;
        public int language_id { get; set; }
        public SubmissionContext context { get; set; } = new();
    }
}
