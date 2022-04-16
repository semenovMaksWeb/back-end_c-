using back_end.Collection;
using MongoDB.Driver;
using MongoDB.Entities;

namespace back_end.Map
{
    public class NameComponentsMap
    {
        async public Task start()
        {
            await DB.DropCollectionAsync<NameComponentsCollection>();
            await DB.CreateCollectionAsync<NameComponentsCollection>(o =>
            {
                o.Collation = new Collation("es");
                o.Capped = false;
            });
            var api_type = new[] {
                new NameComponentsCollection{ name = "table", ID="625a910475263795c98b9d05" },
                new NameComponentsCollection{ name = "button", ID="625a910475263795c98b9d06"},
                new NameComponentsCollection{ name = "container",ID="625a910475263795c98b9d07"},
                new NameComponentsCollection{ name = "form",ID="625a910475263795c98b9d08"},
            };
            await api_type.SaveAsync();
        }
    }
}
