using Word = Microsoft.Office.Interop.Word;

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
        /// <returns>Возвращает директиву файла</returns>
        public string getDirective(string param, string path)
        {
            string directive = $"{Environment.CurrentDirectory}/";
            switch (param)
            {
                case "global":
                    directive += $"wwwroot/{path}";
                    break;
                case "server":
                    directive += $"Files/{path}";
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

        /// <summary>
        /// Метод который сохраняет файл
        /// </summary>
        /// <param name="content">данные которые нужно записать в файл.</param>
        /// <param name="path">Путь до файла.</param>
        ///
        public void saveFile(string path, string content)
        {
            System.IO.File.WriteAllText(path, content);
        }
        /// <summary>
        /// Метод который прочитывает файл по байтам
        /// </summary>
        /// <param name="path">Путь до файла.</param>
        ///
        public byte[] readFileByte(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }


        /// <summary>
        /// возвращает url путь до места где хранятся template docx
        /// </summary>
        public string getTemplateDocxUrl()
        {
            return $"{Environment.CurrentDirectory}/Files/template/docx";
        }

        /// <summary>
        ///сохраняет новый файл по шаблону docx 
        /// </summary>
        /// <param name="content">массив данных который нужно вставить</param>
        /// <param name="filepath">путь к файлу куда нужно сохранить</param>
        /// <param name="path_template">путь к шаблону на основе которого нужно создать файл</param>
        public void saveTemplateDocx(string path_template, string filepath, Dictionary<string, string> content, Dictionary<string, List<List<string>>> content_table)
        {
            Word._Application oWord = new Word.Application();
            Word._Document oDoc = oWord.Documents.Add(path_template);
            foreach (var item in content)
            {
                oDoc.Bookmarks[item.Key].Range.Text = item.Value;
            }
            saveTemplateDocxTable(oDoc, content_table);
            oDoc.SaveAs(FileName: filepath);
            oDoc.Close();
        }
        /// <summary>
        /// метод который заполняет таблицы в шаблоне
        /// </summary>
        /// <param name="oDoc">ссылка на документ</param>
        /// <param name="content_table">объект таблиц которые нужно заполнить</param>
        public void saveTemplateDocxTable(Word._Document oDoc, Dictionary<string, List<List<string>>> content_table)
        {
            foreach (var table in content_table)
            {
                Word.Range wrdRng = oDoc.Bookmarks[table.Key].Range;
                foreach (List<string> row_table in table.Value)
                {
                    Word.Row row = wrdRng.Rows.Add();
                    int index = 0;
                    foreach (string cells in row_table)
                    {
                        index++;
                        row.Cells[index].Range.Text = cells;
                    }
                }
            }
        }
    }
}
