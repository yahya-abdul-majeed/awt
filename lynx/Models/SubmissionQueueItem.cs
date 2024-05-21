namespace lynx.Models
{
    public class SubmissionQueueItem
    {
        public int submission_id { get; set; }
        public string assignment_name { get; set; } = string.Empty;    
        public string auto_verdict { get;set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public string postmoderation_verdict { get;set; } = string.Empty;
        public DateTime submitted_at { get;set; } 
    }
}
