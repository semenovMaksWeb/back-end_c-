using Newtonsoft.Json;

namespace back_end.Types.Screen
{
    public class TypeScreenApi
    {
        [JsonProperty("screen")]
        public TypeScreen screen { get; set; }
        [JsonProperty("breadcrumbs")]
        public List<TypeBreadcrumbscs> breadcrumbs{ get; set;}

        [JsonProperty("components")]
        public List<TypeComponents> components { get; set; }
    }
}
