using lynx.Models.Auth;

namespace lynx.Models
{
    public class Group
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<User> users { get; set; } = new();
    }
}
