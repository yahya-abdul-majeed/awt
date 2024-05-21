namespace lynx.Models
{
    public class ContestRankingItem
    {
        public int contest_id { get; set; }
        public int user_id { get; set; }
        public string username { get; set; } = string.Empty;
        public int total_points { get;set; }
    }
}
