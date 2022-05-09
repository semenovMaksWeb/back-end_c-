using Newtonsoft.Json;
using System.Text.Json;

namespace back_end.Types.Screen
{
    public class TypeComponents
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("class")]
        public object _class { get; set; }

        [JsonProperty("style")]
        public object style { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
        public List<TypeComponentsApi> api { get; set; }

        [JsonProperty("params")]
        public List<Dictionary<string, TypeComponentsParamsApi>> _params { get; set; }
        public Dictionary<string, dynamic> _params_front { get; set; }

    }
}
