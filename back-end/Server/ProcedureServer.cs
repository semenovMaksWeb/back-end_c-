using System.Text.Json;
namespace back_end.Server
{
    public class ProcedureServer
    {
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
                    return checkStringJson(value);
                default:
                    return value;
            }
        }
        dynamic checkStringJson(dynamic value)
        {
            if (value.ValueKind == JsonValueKind.String)
            {
                value = $"\"{value}\"";
            }
            return value;
        }
    }
}
