using Newtonsoft.Json;

namespace back_end.Types.Screen
{
    public class TypeComponentsParamsApi
    {
        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("value")]
        public string value { get; set; }


        [JsonProperty("type")]
        public string type { get; set; }
    }
}
