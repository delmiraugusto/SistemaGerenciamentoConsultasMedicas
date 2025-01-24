using System.Security.Cryptography;
using System.Text;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Infrastructure.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Infrastructure.Repository;

public class AuthRepository : IAuthRepository
{

    private readonly PostgresConnection _dbconnection;

    public AuthRepository(PostgresConnection dbconnection)
    {
        _dbconnection = dbconnection;
    }

    public async Task<UserViewDTO?> FindByEmail(string email)
    {
        var query = @"select id, email, passwordhash, type, name from user_view UV where UV.email = @Email";

        try
        {
            using (var connection = _dbconnection.GetConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync(query, new { Email = email });

                if (result == null)
                    return null;

                return new UserViewDTO(
                    result.id,
                    result.passwordhash,
                    result.name,
                    result.type
                    );
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Erro ao Buscar user por email - {ex.Message}", ex);
        }
    }

    public string CreatePassword(string password)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        return hashedPassword;
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }
}
