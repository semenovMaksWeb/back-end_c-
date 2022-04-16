using MongoDB.Entities;
namespace back_end.Collection
{
    [Collection("name_components_form")]
    public class NameComponentsFormCollection : Entity
    {
        public string name { get; set; }
    }
}
