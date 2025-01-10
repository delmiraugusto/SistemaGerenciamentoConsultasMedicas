using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure;

public class PatientRepository : IPatientRepository
{

    private readonly PostgresConnection _dbConnection;

    public PatientRepository(PostgresConnection dbConnection)
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

    public async Task<Patient> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Patient WHERE id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            var patient = await connection.QueryFirstOrDefaultAsync<Patient>(query, new { Id = id });
            return patient == null ? throw new KeyNotFoundException($"Paciente não encontrado.") : patient;
        }
    }

    public async Task<Patient> GetByEmailAsync(string email)
    {
        var query = "SELECT * FROM Patient WHERE id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            var patient = await connection.QueryFirstOrDefaultAsync<Patient>(query, new { Email = email });
            return patient == null ? throw new KeyNotFoundException($"Paciente não encontrado.") : patient;
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

    public async Task<Patient> AuthenticationAsync(string email, string password)
    {
        var patient = await GetByEmailAsync(email);
        if (patient == null)
        {
            return null;
        }
        if (!VerifyPassword(password, patient.PasswordHash))
        {
            return null;
        }
        return patient;
    }

    public static string CreatePassword(string password)
    {
        using (var shuffle = new HMACSHA512())
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hash = shuffle.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hash);
        }
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        var hash = Convert.FromBase64String(storedHash);
        using (var shuffle = new HMACSHA512())
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var translate = shuffle.ComputeHash(passwordBytes);

            return translate.SequenceEqual(hash);
        }
    }

}
