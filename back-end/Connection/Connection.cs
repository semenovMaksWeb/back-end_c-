using Npgsql;

namespace back_end.Connection
{
    public class Connection
    {
        static public NpgsqlConnection connMain =
     new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;" +
                          "Password=postgres;Database=famneja;");
    }
}
