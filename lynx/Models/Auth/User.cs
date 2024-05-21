﻿namespace lynx.Models.Auth
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email {  get; set; }
        public string password { get; set; }
        public List<string> roles { get; set; } = new();
    }
}
