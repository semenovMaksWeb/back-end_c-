using back_end.Dto;
using back_end.Server;
using Microsoft.AspNetCore.Mvc;
using Word = Microsoft.Office.Interop.Word;

namespace back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileControllers : ControllerBase
    {
        private readonly ILogger<FileControllers> _logger;
        private readonly FileServer fileServer = new FileServer();
        public FileControllers(ILogger<FileControllers> logger)
        {
            _logger = logger;  
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
           string directive = fileServer.getDirective(fileGetConfigDto.directive, fileGetConfigDto.path);
           string filepath = fileServer.getUrlFile(directive, fileGetConfigDto.name);
            _logger.LogInformation($"/file, Получить файл url- {filepath}");
            if (!fileServer.checkFile(filepath)) {
                return NotFound("Файл не найден!");
           }
            byte[] mas = fileServer.readFileByte(filepath);
            return File(mas, fileGetConfigDto.type, fileGetConfigDto.name);
        }
        /// <summary>
        ///  API точка, которая сохраняет новый файл
        /// </summary>
        /// <param name="fileSaveConfigDto"></param>
        [HttpPost("/file/save")]
        public void fileSave(FileSaveConfigDto fileSaveConfigDto)
        {
            string directive = fileServer.getDirective(fileSaveConfigDto.directive, fileSaveConfigDto.path);
            string filepath = fileServer.getUrlFile(directive, fileSaveConfigDto.name);
            fileServer.saveFile(filepath, fileSaveConfigDto.content);
        }


        [HttpPost("/file/template/docx")]
        public ActionResult<long> templateDocx(FileTemplateDocxDto fileTemplateDocxDto)
        {
            string directive = fileServer.getDirective(fileTemplateDocxDto.directive, fileTemplateDocxDto.path);
            string filepath = fileServer.getUrlFile(directive, fileTemplateDocxDto.name);
            string template_filepath = $"{fileServer.getTemplateDocxUrl()}/{fileTemplateDocxDto.template_name}";
            _logger.LogInformation($"/file/template/docx, path_template - {template_filepath}");
            _logger.LogInformation($"/file/template/docx, filepath_save - {filepath}");
            if (!fileServer.checkFile(template_filepath))
            {
                return NotFound("Файл с  не найден!");
            }
            fileServer.saveTemplateDocx(template_filepath, filepath);
            byte[] mas = fileServer.readFileByte(filepath);
            return File(mas, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileTemplateDocxDto.name); 
        }
    }
}

/*
 * file/
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

 * file/save
 * {
  "path": "",
  "name": "251.json",
  "directive": "server",
  "content": "{f:1, d:2, da:51, z:1}"
}
{
  "template_name": "theme.dotx",
  "path": "",
  "name": "theme15.docx",
  "directive": "server"
}


 */