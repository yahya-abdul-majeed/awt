namespace lynx.Models
{
    public class Comment
    {
        public int id { get; set; }
        public int? parent_id { get; set; } = null;
        public int submission_id { get; set; }
        public int user_id { get; set; }
        public string comment_text { get; set; } = string.Empty; 
    }
}
