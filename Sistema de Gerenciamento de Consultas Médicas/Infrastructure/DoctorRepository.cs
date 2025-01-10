using System;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure;

public class DoctorRepository : IDoctorRepository
{
    private readonly PostgresConnection _dbConnection;
    
    public DoctorRepository(PostgresConnection dbConnection)
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

    public async Task<Doctor> GetByEmailAsync(string email)
    {
        var query = "SELECT * FROM Patient WHERE id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            var doctor = await connection.QueryFirstOrDefaultAsync<Doctor>(query, new { Email = email });
            return doctor == null ? throw new KeyNotFoundException($"Médico não encontrado.") : doctor;
        }
    }

    public async Task<Doctor> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Doctor WHERE id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            var doctor = await connection.QueryFirstOrDefaultAsync<Doctor>(query, new { Id = id });
            return doctor == null ? throw new KeyNotFoundException($"Médico não encontrado.") : doctor;
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

    public static string CreatePassword(string password)
    {
        using (var shuffle = new HMACSHA512()){
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

    public async Task<Doctor> AuthenticationAsync(string email, string password)
    {
        var doctor = await GetByEmailAsync(email);
        if (doctor == null)
        {
            return null;
        }
        if (!VerifyPassword(password, doctor.PasswordHash))
        {
            return null;
        }
        return doctor;
    }

}
