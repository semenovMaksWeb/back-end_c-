using MongoDB.Entities;
namespace back_end.Collection
{
    public class ApiComponentsCollection : Entity
    {
        public string name { get; set; }
        public One<ApiTypeCollection> url { get; set; }
    }
}
