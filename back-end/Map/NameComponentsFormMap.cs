using back_end.Collection;
using MongoDB.Driver;
using MongoDB.Entities;

namespace back_end.Map
{
    public class NameComponentsFormMap
    {
        async public Task start()
        {
            await DB.DropCollectionAsync<NameComponentsFormCollection>();
            await DB.CreateCollectionAsync<NameComponentsFormCollection>(o =>
            {
                o.Collation = new Collation("es");
                o.Capped = false;
            });
            var api_type = new[] {
                new NameComponentsFormCollection{ name = "fieldset", ID = "625a910475263795c98b9cff" },
                new NameComponentsFormCollection{ name = "button", ID = "625a910475263795c98b9d00" },
                new NameComponentsFormCollection{ name = "container",ID = "625a910475263795c98b9d01"},
                new NameComponentsFormCollection{ name = "input",ID = "625a910475263795c98b9d02"},
                new NameComponentsFormCollection{ name = "select",ID = "625a910475263795c98b9d03"},
                new NameComponentsFormCollection{ name = "rows",ID = "625a910475263795c98b9d04"},
            };
            await api_type.SaveAsync();
        }
    }
}
