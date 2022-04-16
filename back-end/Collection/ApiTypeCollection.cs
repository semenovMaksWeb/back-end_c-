using MongoDB.Entities;

namespace back_end.Collection
{
    [Collection("api_type")]
    public class ApiTypeCollection: Entity
    {
        public string name { get; set; }
    }
}
