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
        // 
        public async Task<List<dynamic>> procedure(string name, dynamic json)
        {
            return await procedureServer.procedure(name, json);
        }
    }
}
