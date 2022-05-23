using back_end.Server;
using Microsoft.AspNetCore.Mvc;
namespace back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentControllers : ControllerBase
    {
        private readonly ILogger<FileControllers> _logger;
        private readonly ComponentServer componentServer = new ComponentServer();
        public ComponentControllers(ILogger<FileControllers> logger)
        {
            _logger = logger;
        }
        [HttpPost("/screen")]
        public async Task<dynamic> screen(string url)
        {
            _logger.LogInformation("Authorization-{0}", Request.Headers.Authorization);
            return await componentServer.screenGet(url);
        }
    }
}
