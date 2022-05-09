using Npgsql;
using System.Text.Json;
using System.Dynamic;
namespace back_end.Server
{
    public class ProcedureServer
    {
        public NpgsqlConnection db = Connection.Connection.connMain;

        public async Task<List<dynamic>> convertBd(string sql)
        {
            List<dynamic> result = new List<dynamic>();
            await db.OpenAsync();
            NpgsqlCommand sql_db = new NpgsqlCommand(sql, db);
            NpgsqlDataReader dr = sql_db.ExecuteReader();
            int count = dr.FieldCount;
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
            await db.CloseAsync();
            return result;
        }

        //
        public async Task<List<dynamic>> procedure(string name, dynamic json)
        {
            string _params = paramsGenerator(json);
            string sql = $"select * from {name}({ _params })";
            return await convertBd(sql);
    
        }

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
        string paramsGeneratorString(dynamic value)
        {
           return $"\"{value}\"";
        }
    }
}
