using System.Numerics;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure;

public class PacienteRepository : IPacienteRepository
{

    private readonly PostgresConnection _dbConnection;

    public PacienteRepository(PostgresConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task <IEnumerable<Patient>> GetAllAsync()
    {
        var query = "SELECT * FROM Patient";
        using (var connection = _dbConnection.GetConnection())
        {
            return await connection.QueryAsync<Patient>(query);
        }
    }

    public async Task AddAsync(Patient patient)
    {
        var query = "INSERT INTO Patient (Name, Email, Password, Telephone, Age) " +
    "VALUES (@Name, @Email, @Password, @Telephone, @Age)";
        using (var connection = _dbConnection.GetConnection())
        {
            await connection.ExecuteAsync(query, patient);
        }
    }

    public async Task UpdateAsync(Patient patient)
    {
        var query = "UPDATE Patient SET Telephone = @Telephone, Age = @Age, Name = @Name Where id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            await connection.ExecuteAsync(query, patient);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var query = "DELETE FROM Patient WHERE Id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });

            if (affectedRows == 0)
            {
                throw new InvalidOperationException($"Paciente não encontrado.");
            }
        }
    }



}
