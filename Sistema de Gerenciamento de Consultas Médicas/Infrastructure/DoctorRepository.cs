using System;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly PostgresConnection _dbConnection;

        public DoctorRepository(PostgresConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            var query = @"
            SELECT 
                d.Id,
                d.Name,
                d.Email,
                d.PasswordHash,
                d.Telephone,
                d.Crm,
                d.IsActive,
                d.Funcao,
                d.SpecialtyId,
                s.Name AS SpecialtyName
            FROM 
                Doctor d
            LEFT JOIN 
                Specialty s 
            ON 
                d.SpecialtyId = s.Id";

            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    return await connection.QueryAsync<Doctor>(query);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao buscar os médicos. Consulte o log para mais detalhes." + ex.Message);
            }
        }



        public async Task<Doctor> GetByEmailAsync(string email)
        {
            var query = "SELECT * FROM Doctor WHERE email = @Email"; // Correção aqui
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
            var query = @"
        INSERT INTO Doctor (Name, Email, PasswordHash, Telephone, Crm, SpecialtyId, IsActive) 
        VALUES (@Name, @Email, @PasswordHash, @Telephone, @Crm, @SpecialtyId, @IsActive)";
            using (var connection = _dbConnection.GetConnection())
            {
                try
                {
                    await connection.ExecuteAsync(query, new
                    {
                        doctor.Name,
                        doctor.Email,
                        doctor.PasswordHash,
                        doctor.Telephone,
                        doctor.Crm,
                        doctor.Specialty,
                        doctor.IsActive
                    });
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Erro ao adicionar um novo médico.", ex);
                }
            }
        }


        public async Task UpdateAsync(Doctor doctor)
        {
            var query = @"
                    UPDATE Doctor 
                    SET Name = @Name, 
                        Email = @Email, 
                        PasswordHash = @PasswordHash, 
                        Telephone = @Telephone, 
                        Crm = @Crm, 
                        SpecialtyId = @SpecialtyId, 
                        IsActive = @IsActive 
                    WHERE Id = @Id";
            using (var connection = _dbConnection.GetConnection())
            {
                try
                {
                    var affectedRows = await connection.ExecuteAsync(query, new
                    {
                        doctor.Name,
                        doctor.Email,
                        doctor.PasswordHash,
                        doctor.Telephone,
                        doctor.Crm,
                        doctor.Specialty,
                        doctor.IsActive,
                        doctor.Id
                    });

                    if (affectedRows == 0)
                    {
                        throw new KeyNotFoundException("Médico não encontrado para atualização.");
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Erro ao atualizar o médico.", ex);
                }
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
}
