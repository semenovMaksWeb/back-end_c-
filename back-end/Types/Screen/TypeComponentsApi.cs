using System.Text.Json.Serialization;

namespace back_end.Types.Screen
{
    public class TypeComponentsApi
    {
        
        public string? url { get; set; }
        public string? type { get; set; }

        [JsonPropertyName("params")]
        public Dictionary<string, TypeComponentsApiParams>? _params { get; set; }

    }
}
