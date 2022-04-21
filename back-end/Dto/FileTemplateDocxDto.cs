namespace back_end.Dto
{
    public class FileTemplateDocxDto
    {
        public string template_name { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public string directive { get; set; }

        public Dictionary<string, string> content { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, List<List<string>>> content_table { get; set; } = new Dictionary<string, List<List<string>> > ();
    }
}
