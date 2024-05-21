namespace ibex.Models.DTO
{
    public class SkeletonDTO
    {
        public int id { get; set; }
        public int testing_fw_id { get; set; }
        public int language_id { get; set; }
        public string code_before { get; set; }
        public string code_after { get; set; }
        public string skeleton_name { get; set; }
        public string testing_fw_name { get; set;}
        public string language_name { get; set; }
    }
}
