using Npgsql;
using System.Text.Json;
using System.Dynamic;
namespace back_end.Server
{
    public class ProcedureServer
    {
        public NpgsqlConnection db = Connection.Connection.connMain;

        /// </summary>
        ///  Метод, который вызывает функцию бд и парсит её ответ
        /// <param name="sql"> sql запрос</param>
        /// <returns>возвращает json ответ функции бд</returns>
        public async Task<List<dynamic>> convertBd(string sql)
        {
            List<dynamic> result = new List<dynamic>();
            db.Open();
            NpgsqlCommand sql_db = new NpgsqlCommand(sql, db);
            NpgsqlDataReader dr = sql_db.ExecuteReader();
            int count = dr.FieldCount;
            // ответ с бд прогнать
            while (dr.Read())
            {
                dynamic _object = new ExpandoObject();
                for (int i = 0; i < count; i++)
                {
                    TypeCode type = Type.GetTypeCode(dr.GetFieldType(i));     
                    ((IDictionary<String, dynamic>)_object).Add(dr.GetName(i), bdGenerator(type, i, dr));
                }
                result.Add(_object);
            }
            db.Close();
            return result;
        }

        /// <summary>
        ///  Метод, который генерирует строку запроса sql и вызывает convertBd
        /// </summary>
        /// <param name="name"> имя/путь до функции бд</param>
        /// <param name="json">параметры передающий в функцию бд</param>
        /// <returns>возвращает json ответ функции бд</returns>
        public async Task<List<dynamic>> procedure(string name, dynamic json)
        {
            string _params = paramsGenerator(json);
            string sql = $"select * from {name}({ _params })";
            return await convertBd(sql);
        }

        /// <summary>
        /// Метод, конвертирует ответ функции бд под нужный тип данных
        /// </summary>
        /// <param name="type">тип переменной</param>
        /// <param name="index">index</param>
        /// <param name="dr">NpgsqlDataReader нужно для парса бд</param>
        /// <returns></returns>
        public dynamic bdGenerator(TypeCode type, int index, NpgsqlDataReader dr)
        {
            switch (type)
            {
                case TypeCode.String:
                    return dr.GetString(index);
                case TypeCode.Int16:
                    return dr.GetInt16(index);
                case TypeCode.Int32:
                    return dr.GetInt32(index);
                case  TypeCode.Int64:
                    return dr.GetInt64(index);
                default:  
                    return null;
            }
        }

        /// <summary>
        /// Метод генерирует строку которая будет передана как параметр для функции бд
        /// </summary>
        /// <param name="json">объект с параметрами для фунции бд</param>
        /// <returns>Возвращает строку которая передает параметры в функцию бд</returns>
        public string paramsGenerator(dynamic json)
        {
            string _params = "";
            foreach (var k in json.EnumerateObject())
            {
                if (_params != "")
                {
                    _params += ", ";
                }
                dynamic value = checkParams(k.Value);
                _params += $"{k.Name} => {value}";
            }
            return _params;
        }

        /// <summary>
        /// Метод проверять нужно ли обработать параметр передающийся в функцию бд
        /// </summary>
        /// <param name="value">параметр который нужно обработать</param>
        /// <returns>возвращает новое валидное значение</returns>
        dynamic checkParams(dynamic value)
        {
            switch (value.ValueKind)
            {
                case JsonValueKind.String:
                    return paramsGeneratorString(value);
                default:
                    return value;
            }
        }
        /// <summary>
        /// Метод конвертирует параметр передающий в функции в 'value'
        /// </summary>
        /// <param name="value">параметр который нужно обработать</param>
        /// <returns>возвращает новую строку</returns>
        string paramsGeneratorString(dynamic value)
        {
           return $"\'{value}\'";
        }
    }
}
