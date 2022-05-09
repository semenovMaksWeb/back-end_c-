using Npgsql;
using System.Dynamic;
using back_end.Types.Screen;


namespace back_end.Server
{
    public class ComponentServer
    {
        public NpgsqlConnection db = Connection.Connection.connMain;
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
            json = componentsParseParams(json);
            db.Close();
            return json;
        }

        public TypeScreenApi componentsParseParams(TypeScreenApi json)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (TypeComponents components in json.components)
            {

                if (components._params == null)
                {
                    continue;
                }
                foreach (Dictionary<string, TypeComponentsParamsApi> _params in components._params)
                {
                    foreach (string key in _params.Keys)
                    {
                        components._params_front.Add(key, checkParamsType(_params[key]));
                    }
                }
                components._params = null;
            }
            return json;
        }


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

    }
}
