using back_end.Collection;
using MongoDB.Driver;
using MongoDB.Entities;

namespace back_end.Map
{
    public class CallbackMap
    {
        async public Task start()
        {
            await DB.DropCollectionAsync<CallbackCollection>();
            await DB.CreateCollectionAsync<CallbackCollection>(o =>
            {
                o.Collation = new Collation("es");
                o.Capped = false;
            });
            var api_type = new[] {
                new CallbackCollection{ name = "api", ID =  "625a910375263795c98b9cf6" },
                new CallbackCollection{ name = "delete_table_row",   ID =  "625a910375263795c98b9cf7"},
                new CallbackCollection{ name = "reset_values_form",ID =  "625a910375263795c98b9cf8"},
                new CallbackCollection{ name = "reset_value_form_key",ID =  "625a910375263795c98b9cf9" },
                new CallbackCollection{ name = "add_table_row",ID =  "625a910375263795c98b9cfa" },
                new CallbackCollection{ name = "router_push",ID =  "625a910375263795c98b9cfb" },
                new CallbackCollection{ name = "update_manual",ID =  "625a910375263795c98b9cfc" },
                new CallbackCollection{ name = "add_rows_values_form",ID =  "625a910375263795c98b9cfd" },
                new CallbackCollection{ name = "delete_rows_values_form",ID = "625a910375263795c98b9cfe" },
            };
            await api_type.SaveAsync();
        }
    }
}
