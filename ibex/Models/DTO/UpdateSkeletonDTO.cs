﻿namespace ibex.Models.DTO
{
    public class UpdateSkeletonDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code_before { get; set; }
        public string code_after { get; set;}
        public int testing_framework_id { get; set; }
    }
}