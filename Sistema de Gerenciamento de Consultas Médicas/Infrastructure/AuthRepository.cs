using System.Security.Cryptography;
using System.Text;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
using static System.Net.Mime.MediaTypeNames;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Infrastructure;

public class AuthRepository : IAuthRepository
{

    private readonly PostgresConnection _dbconnection;
    private readonly string HASH_SALT_KEY = "SaltFixo1234567890";


    public AuthRepository(PostgresConnection dbconnection)
    {
        _dbconnection = dbconnection;
    }

    public async Task<UserViewDTO?> FindByEmail(string email)
    {
        var query = @"select id, passwordhash from user_view UV where UV.email = @Email";

        try
        {
            using (var connection = _dbconnection.GetConnection())
            {

                var test = await connection.QueryFirstOrDefaultAsync<UserViewDTO?>(query, new { Email = email });
                return test;
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Erro ao Buscar user por id - {ex.Message}");
        }

    }

        public string CreatePassword(string password)
        {
            var salt = Encoding.UTF8.GetBytes(HASH_SALT_KEY);
            using (var hmac = new HMACSHA512(salt))
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hash = hmac.ComputeHash(passwordBytes);

                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            var hash = Convert.FromBase64String(storedHash);
            var salt = Encoding.UTF8.GetBytes(HASH_SALT_KEY);

            using (var hmac = new HMACSHA512(salt))
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var computedHash = hmac.ComputeHash(passwordBytes);

                return computedHash.SequenceEqual(hash);
            }
        }



}
