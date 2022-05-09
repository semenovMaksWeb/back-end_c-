using Newtonsoft.Json;
using Npgsql;
using System.Dynamic;
using System.Text.Json;
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
            while(dr.Read())
            {
                result = dr.GetValue(0);
            }
            JsonElement json_element = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(result);

            TypeScreenApi json =
                JsonConvert.DeserializeObject<TypeScreenApi>(result);
            json = componentsParseParams(json);


            db.Close();
            return json;
        }
    
    
        public TypeScreenApi componentsParseParams(TypeScreenApi json)
        {
            foreach(TypeComponents components in json.components)
            {

               foreach(Dictionary<string, TypeComponentsParamsApi> _params in components._params)
               {
                    foreach (string key in _params.Keys)
                    {
                        // сломанное 
                        /*
                         * #TODO баг ниже
                         * *
                         */
                        components._params_front.Add(key, _params[key]);
                    }
               }
            }
            return json;
        }    
    }
}
