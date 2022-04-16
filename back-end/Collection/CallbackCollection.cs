using MongoDB.Entities;

namespace back_end.Collection
{
    [Collection("callback_name")]
    public class CallbackCollection : Entity
    {
        public string name { get; set; }
    }
}
