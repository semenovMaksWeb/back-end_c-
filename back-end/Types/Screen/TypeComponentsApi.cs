using Newtonsoft.Json;

namespace back_end.Types.Screen
{
    public class TypeComponentsApi
    {
        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("result")]
        public string result { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("index")]
        public int index { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("type_var")]
        public string type_var { get; set; }

    }
}
