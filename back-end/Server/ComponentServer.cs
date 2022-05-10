using Npgsql;
using System.Dynamic;
using back_end.Types.Screen;


namespace back_end.Server
{
    public class ComponentServer
    {
        public NpgsqlConnection db = Connection.Connection.connMain;
        
        /// <summary>
        /// Метод точка входа обработки скрина
        /// </summary>
        /// <param name="id">id скрина</param>
        /// <returns> возвращает конфиг скрина</returns>
        public dynamic screenGet(int id)
        {
            dynamic result = new ExpandoObject();
            db.Open();
            string sql = @"select * from components.screen_platform_get(@id)";
            NpgsqlCommand sql_db = new NpgsqlCommand(sql, db);
            sql_db.Parameters.AddWithValue("@id", id);
            NpgsqlDataReader dr = sql_db.ExecuteReader();
            while (dr.Read())
            {
                result = dr.GetValue(0);
            }
            TypeScreenApi json = System.Text.Json.JsonSerializer.Deserialize<TypeScreenApi>(result);
            componentsParse(json);
            db.Close();
            return json;
        }

        /// <summary>
        /// Парсит комноненты скрина под определеную струкутру для фронта
        /// </summary>
        /// <param name="json">json конфиг скрина</param>
        public void componentsParse(TypeScreenApi json)
        {
            // прогнать компоненты
            foreach (string key_components in json.components.Keys)
            {
                paramsConvert(json, key_components);
                List<string> idDelete = schemaFormConvert(json, key_components);
                deleteIdComponentSchemaForm(json, key_components, idDelete);
                json.components[key_components]._params = null;
                json.components[key_components].schema_form = null;
            }
        }
        /// <summary>
        /// конвертирует schemaForm у компонента type=form
        /// </summary>
        /// <param name="json">json конфиг скрина</param>
        /// <param name="key_components">ключ компонента</param>
        /// <returns></returns>
        public List<string> schemaFormConvert(TypeScreenApi json, string key_components)
        {
            List<string> idDeleteComponents = new List<string>();
            string key;
            if (json.components[key_components].type == "form" && json.components[key_components].schema_form != null)
            {
                // прогнать схема формы
                foreach (TypeComponentsSchemaForm schema_form in json.components[key_components].schema_form)
                {
                    if (schema_form.id_parent == null)
                    {
                        key = key_components;
                    }
                    else
                    {
                        key = schema_form.id_parent.ToString();
                    }
                    checkChildrenSchemaForm(json.components[key]);
                    json.components[key].children.Add(json.components[schema_form.id_components.ToString()]);
                    idDeleteComponents.Add(schema_form.id_components.ToString());
                }
            }
            return idDeleteComponents;
        }

        /// <summary>
        /// проверяет есть ли у компонента ключ children и добавляет его если нет
        /// </summary>
        /// <param name="components">Сслыка на компонент</param>
        public void checkChildrenSchemaForm(TypeComponents components)
        {
            if (components != null && components.children == null)
            {
                components.children = new List<TypeComponents>();
            }
        }

        /// <summary>
        /// конвертирует параметры
        /// </summary>
        /// <param name="json">json конфиг скрина</param>
        /// <param name="key_components">ключ компонента</param>
        public void paramsConvert(TypeScreenApi json, string key_components)
        {
            if (json.components[key_components]._params != null)
            {
                // прогнать параметры
                foreach (string key_params in json.components[key_components]._params.Keys)
                {
                    json.components[key_components].params_front.Add(key_params, checkParamsType(json.components[key_components]._params[key_params]));
                }
            }
        }
        
        /// <summary>
        /// меняет тип данных на указанный
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic checkParamsType(TypeComponentsParamsApi param)
        {
            switch (param.type)
            {
                case "int":
                    return Convert.ToInt32(param.value);

                case "boolean":
                    if(param.value == "true")
                    {
                        return true;
                    }
                    return false;

                default:
                    return param.value;
            }
        }

        /// <summary>
        /// Удаляет компоненты которые были прикручены к форме (оптимизация удаление не нужных элеметов)
        /// </summary>
        /// <param name="json">json конфиг скрина</param>
        /// <param name="key_components">ключ компонента</param>
        /// <param name="idDelete">массив id компонентов который прикрутились к форме</param>
        public void deleteIdComponentSchemaForm(TypeScreenApi json, string key_components, List<string> idDelete)
        {
            foreach(string id in idDelete)
            {
                json.components.Remove(id);
            }
        }

    }
}
