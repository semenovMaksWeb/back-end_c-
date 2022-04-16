using back_end.Collection;
using MongoDB.Driver;
using MongoDB.Entities;
namespace back_end.Map
{
    public class ApiComponentsMap
    {
        async public Task start() {
            await DB.DropCollectionAsync<ApiComponentsCollection>();
            await DB.CreateCollectionAsync<ApiComponentsCollection>(o =>
            {
                o.Collation = new Collation("es");
                o.Capped = false;
            });
        }
    }
}
