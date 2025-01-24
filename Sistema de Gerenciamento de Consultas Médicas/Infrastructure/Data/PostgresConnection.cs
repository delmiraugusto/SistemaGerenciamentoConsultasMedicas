using System.Data;
using Npgsql;
namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Infrastructure.Data;

public class PostgresConnection
{
    private readonly string _connectionString;

    public PostgresConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
