using System.Text.Json.Serialization;

namespace back_end.Types.Screen
{
    public class TypeComponentsCallback
    {
        public int id { get; set; }

        public string name { get; set; }

        [JsonPropertyName("params")]
        public dynamic _params { get; set; }

        public int order { get; set; }
    }
}
