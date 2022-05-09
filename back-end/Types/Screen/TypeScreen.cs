using Newtonsoft.Json;

namespace back_end.Types.Screen
{
    public class TypeScreen
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }
    }
}
