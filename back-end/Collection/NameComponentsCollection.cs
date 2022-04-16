using MongoDB.Entities;
namespace back_end.Collection
{
    [Collection("name_components")]
    public class NameComponentsCollection : Entity
    {
        public string name { get; set; }
    }
}
