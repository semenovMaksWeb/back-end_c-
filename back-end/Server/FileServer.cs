using Microsoft.AspNetCore.Hosting; // для IWebHostEnvironment

namespace back_end.Server
{
    /// <summary>
    /// Класс, который представляет сервер работы с файлами.
    /// </summary>
    public class FileServer
    {
        /// <summary>
        /// Метод, который генерирует url до файла
        /// </summary>
        /// <param name="path">Путь до файла.</param>
        /// <param name="name">Имя файла.</param>
        /// <returns>Возвращает url до файла</returns>
        public string getUrlFile(string path, string name)
        {
            return Path.Combine(path, name);
        }
        /// <summary>
        /// Метод, который генерирует url до директивы
        /// </summary>
        /// <param name="param">Ключевое слово определяющие директиву.</param>
        /// <param name="path">Путь до файла.</param>
        /// <param name="_appEnvironment">Служебная переменная текущая директива сервера.</param>
        /// <returns>Возвращает директиву файла</returns>
        public string getDirective(string param, string path, IWebHostEnvironment _appEnvironment) {
            string directive = _appEnvironment.ContentRootPath;
            switch (param)
            {
                case "global":
                    directive += "wwwroot/" + path;
                    break;
                case "server":
                    directive += "Files/" + path;
                    break;
            }
            return directive;
        }

        /// <summary>
        /// Метод, который проверят наличия файла  
        /// </summary>
        /// <param name="path">Путь до файла.</param>
        /// <param name="name">Имя файла.</param>
        /// <returns>Возвращает флаг существует ли файд</returns>
        public bool checkFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            return false;
        }
       
    }
}
