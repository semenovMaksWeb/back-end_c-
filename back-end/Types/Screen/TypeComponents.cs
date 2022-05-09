using System.Text.Json.Serialization;

namespace back_end.Types.Screen
{
    public class TypeComponents
    {
 
        public int id { get; set; }

        [JsonPropertyName("class")]
        public Dictionary<string, dynamic> _class { get; set; }
 
         public Dictionary<string, dynamic> style { get; set; }

 
        public string type { get; set; }
        public List<TypeComponentsApi> api { get; set; }

        [JsonPropertyName("params")]
        public List<Dictionary<string, TypeComponentsParamsApi>> _params { get; set; }
        public Dictionary<string, dynamic> _params_front { get; set; } = new Dictionary<string, dynamic>();

    }
}
