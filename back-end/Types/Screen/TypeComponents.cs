using System.Text.Json.Serialization;

namespace back_end.Types.Screen
{
    public class TypeComponents
    {
 
        public int id { get; set; }

        [JsonPropertyName("class")]
        public Dictionary<string, dynamic> _class { get; set; }
 
         public Dictionary<string, dynamic> style { get; set; }

        [JsonPropertyName("event")]
        public Dictionary<string, List<TypeComponentsCallback>> _event { get; set; }

        [JsonPropertyName("schema_table")]
        public Dictionary<string, TypeComponentsSchemaTable> schema_table { get; set; }

        [JsonPropertyName("schema_form")]
        public List<TypeComponentsSchemaForm> schema_form { get; set; }

        public List<TypeComponents> children { get; set; } 


        public string type { get; set; }
        
        public TypeComponentsApi api { get; set; }

        [JsonPropertyName("params")]
        public Dictionary<string, TypeComponentsParamsApi> _params { get; set; }
        
        public Dictionary<string, dynamic> params_front { get; set; } = new Dictionary<string, dynamic>();



    }
}
