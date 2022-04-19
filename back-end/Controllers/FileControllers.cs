using back_end.Dto;
using back_end.Server;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileControllers : ControllerBase
    {
        private readonly ILogger<FileControllers> _logger;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly FileServer fileServer = new FileServer();
        public FileControllers(ILogger<FileControllers> logger, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
        }
        /// <summary>
        ///  API точка, которая возвращает существующие файлы
        /// </summary>
        /// <param name="fileGetConfigDto"></param>
        /// <returns> успех - возвращает файл, ошибка - возвращает текст ошибки</returns>
        [HttpPost("/file")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<long> fileGet(FileGetConfigDto fileGetConfigDto)
        {
           string directive = fileServer.getDirective(fileGetConfigDto.directive, fileGetConfigDto.path, _appEnvironment);
           string filepath = fileServer.getUrlFile(directive, fileGetConfigDto.name);
            _logger.LogInformation("/file, Получить файл url- " + filepath);
            if (!fileServer.checkFile(filepath)) {
                return NotFound("Файл не найден!");
           }
            byte[] mas = System.IO.File.ReadAllBytes(filepath);
            return File(mas, fileGetConfigDto.type, fileGetConfigDto.name);
        }
    }
}

/*
{
  "path": "txt",
  "name": "1.txt",
  "type": "text/plain",
  "directive": "global"
}
{
  "path": "",
  "name": "1.json",
  "type": "application/json",
  "directive": "server"
}
 */