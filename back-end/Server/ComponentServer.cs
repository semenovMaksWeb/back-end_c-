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
        public async Task<dynamic> screenGet(string url)
        {
            dynamic result = new ExpandoObject();
            await db.OpenAsync();
            string sql = @"select * from components.screen_platform_get_url(@url)";
            NpgsqlCommand sql_db = new NpgsqlCommand(sql, db);
            sql_db.Parameters.AddWithValue("@url", url);
            NpgsqlDataReader dr = sql_db.ExecuteReader();
            while (dr.Read())
            {
                result = dr.GetValue(0);
            }
            TypeScreenApi json = System.Text.Json.JsonSerializer.Deserialize<TypeScreenApi>(result);
            componentsParse(json);
            await db.CloseAsync();
            return json;
        }

        /// <summary>
        /// обработка калбеков у компонентов
        /// </summary>
        /// <param name="json">json конфиг скрина</param>
        /// <param name="key_components">ключ компонента</param>
        public void CallbackConvert(TypeScreenApi json, string key_components)
        {
            Dictionary<string, List<TypeComponentsCallback>> _event = json.components[key_components]._event;
            if (json.components[key_components]._event != null)
            {
                foreach(string event_key in _event.Keys)
                {
                    _event[event_key].Sort((x, y) => x.order - y.order);
                }
            }
        }

        public void componentsTable(TypeScreenApi json, string key_components)
        {
            Dictionary<string, TypeComponentsParamsApi> _params = json.components[key_components]._params;
            Dictionary<string, dynamic> params_front = json.components[key_components].params_front;
            if(_params != null && json.components[key_components].type == "table")
            {
                if (!_params.ContainsKey("key_main"))
                {
                    params_front.Add("key_main", "id");
                }
            }
        }
        
        public void breadcrumbsConvert(TypeScreenApi json)
        {
            if (json.breadcrumbs != null)
            {
                json.breadcrumbs.Sort((x, y) => x.order - y.order);
                json.breadcrumbs[json.breadcrumbs.Count-1].url = null;
            }
        }

        /// <summary>
        /// Обработка schemaTable у таблицы
        /// </summary>
        /// <param name="json">json конфиг скрина</param>
        /// <param name="key_components">ключ компонента</param>
        public void schemaTableConvert(TypeScreenApi json, string key_components)
        {
            if(json.components[key_components].type == "table" && json.components[key_components].schema_table != null)
            {
                //json.components[key_components].schema_table.Sort((x, y) => x.order - y.order);
            }
        }

        /// <summary>
        /// Функция обработки формы
        /// </summary>
        /// <param name="json">json конфиг скрина</param>
        /// <param name="key_components">ключ компонента</param>
        public void componentsForm(TypeScreenApi json, string key_components)
        {
            if(json.components[key_components].type == "form")
            {
                json.components[key_components].params_front.Add("errors", new Dictionary<string, dynamic>());
            }
        }

        /// <summary>
        /// Парсит комноненты скрина под определеную струкутру для фронта
        /// </summary>
        /// <param name="json">json конфиг скрина</param>
        public void componentsParse(TypeScreenApi json)
        {
            breadcrumbsConvert(json);
            if (json.components != null)
            {
                // прогнать компоненты
                foreach (string key_components in json.components.Keys)
                {
                    componentsTable(json, key_components);
                    componentsForm(json, key_components);
                    CallbackConvert(json, key_components);
                    paramsConvert(json, key_components);
                    json.components[key_components]._params = null;
                }
                // прогнать компоненты schema_form
                foreach (string key_components in json.components.Keys)
                {
                    List<string> idDelete = schemaFormConvert(json, key_components);
                    deleteIdComponentSchemaForm(json, key_components, idDelete);
                    json.components[key_components].schema_form = null;
                }
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
            Dictionary<string, TypeComponentsParamsApi> _params = json.components[key_components]._params;
            Dictionary<string, dynamic> params_front = json.components[key_components].params_front;
            if (_params != null)
            {
                // прогнать параметры
                foreach (string key_params in _params.Keys)
                {
                    // url есть
                    if (_params[key_params].url != null)
                    {
                        paramsCreateUrl(_params, params_front, key_params);
                    }
                    // url нет
                    else
                    {
                        params_front.Add(key_params, checkParamsType(_params[key_params]));
                    }
                }
            }
        }

        /// <summary>
        /// Функция подготавливает вложенность для компонентов
        /// </summary>
        /// <param name="_params">параметры с бэка</param>
        /// <param name="params_front">параметры для фронта</param>
        /// <param name="key_params">имя параметра</param>
        public void paramsCreateUrl(Dictionary<string, TypeComponentsParamsApi> _params, Dictionary<string, dynamic> params_front, string key_params)
        {
                if (!params_front.ContainsKey(_params[key_params].url))
                {
                    params_front.Add(_params[key_params].url, new Dictionary<string, dynamic>());

                }
                params_front[_params[key_params].url].Add(key_params, checkParamsType(_params[key_params]));
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
                case "object":
                    return System.Text.Json.JsonSerializer.Deserialize<dynamic>(param.value);
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
