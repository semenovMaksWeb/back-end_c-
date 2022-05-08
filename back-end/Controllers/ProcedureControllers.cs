using back_end.Server;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcedureControllers : ControllerBase
    {
        private readonly ILogger<FileControllers> _logger;
        private readonly ProcedureServer procedureServer = new ProcedureServer();
        public ProcedureControllers(ILogger<FileControllers> logger)
        {
            _logger = logger;
        }

        [HttpPost("/procedure")]
        public string procedure(string name, dynamic json)
        {
            string sql = $"select * from {name}({ procedureServer.paramsGenerator(json) })";
            return sql;
        }
    }

}
/*
 {
    "id": 2,
    "name": "test"
 }
 */
