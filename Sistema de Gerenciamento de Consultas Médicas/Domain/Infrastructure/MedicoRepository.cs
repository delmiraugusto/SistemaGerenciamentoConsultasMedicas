using System;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure;

public class MedicoRepository : IMedicoRepository
{
    private readonly PostgresConnection _dbConnection;
    
    public MedicoRepository(PostgresConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        var query = "SELECT * FROM Doctor";
        using (var connection = _dbConnection.GetConnection())
        {
            return await connection.QueryAsync<Doctor>(query);
        }
    }

    public async Task<Doctor> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Doctor WHERE id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            var doctor = await connection.QueryFirstOrDefaultAsync<Doctor>(query, new { Id = id });
            return doctor == null ? throw new KeyNotFoundException($"Médico com o ID {id} não foi encontrada.") : doctor;
        }
    }

    public async Task AddAsync(Doctor doctor)
    {
        var query = "INSERT INTO Doctor (Name, Email, Password, Telephone, Crm, Specialty) " +
            "VALUES (@Name, @Email, @Password, @Telephone, @Crm, @Specialty)";
        using (var connection = _dbConnection.GetConnection())
        {
            await connection.ExecuteAsync(query, doctor);
        }
        
    }

    public async Task UpdateAsync(Doctor doctor)
    {
        var query = "UPDATE Doctor SET Name = @Name, Telephone = @Telephone, IsActive = @IsActive Where id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            await connection.ExecuteAsync(query, doctor);
        }
    }

    public async Task CancelAsync(int id)
    {
        var query = "UPDATE Doctor SET IsActive = 0 WHERE id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Médico não encontrado ou já foi desativado.");
            }
        }
    }

}
