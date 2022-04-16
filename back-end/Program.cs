using back_end.Map;
using back_end.Collection;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// mongo
await DB.InitAsync("system");


var apiTypeCollection = 
    DB.Find<ApiComponentsCollection>()
    .ExecuteAsync();

Map map = new Map();
await map.start();
// mongo
app.MapGet("/", () => apiTypeCollection);

app.Run();
