using back_end.Collection;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Bson;

namespace back_end.Map
{
    public class ApiTypeMap
    {
        async public Task start()
        {
            await DB.DropCollectionAsync<ApiTypeCollection>();
            await DB.CreateCollectionAsync<ApiTypeCollection>(o =>
            {
                o.Collation = new Collation("es");
                o.Capped = false;
            });
            var api_type = new[] {
                new ApiTypeCollection{ name = "get",   ID = "625a8ab762210acb26dd06ff" },
                new ApiTypeCollection{ name = "delete", ID = "625a8ab762210acb26dd0700" },
                new ApiTypeCollection{ name = "post",ID = "625a8ab762210acb26dd0701"},
                new ApiTypeCollection{ name = "put", ID = "625a8ab762210acb26dd0702" }
            };
            await api_type.SaveAsync();
        }
    }
}
