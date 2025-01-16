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
            SELECT *
            FROM Doctor d
            WHERE IsActive = TRUE
            ORDER BY d.Name";
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    return await connection.QueryAsync<Doctor>(query);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao buscar os médicos.");
            }
        }



        public async Task<Doctor> GetByEmailAsync(string email)
        {
            var query = "SELECT * FROM Doctor WHERE email = @Email";
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

        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            var query = 
                "INSERT INTO Doctor (Name, Email, PasswordHash, Telephone, Crm, Specialty, Cpf)" +
                    "VALUES (@Name, @Email, @PasswordHash, @Telephone, @Crm, @Specialty, @Cpf)" + 
                "RETURNING Id";

            using (var connection = _dbConnection.GetConnection())
            {
                try
                {

                    var newDoctorId = await connection.ExecuteScalarAsync<int>(query, doctor);

                    doctor.Id = newDoctorId;
                    if (newDoctorId <= 0)
                    {
                        throw new ApplicationException("Erro ao gerar o ID do medico.");
                    }
                    return doctor;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Erro ao adicionar médico." + ex.Message);
                }
            }
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            var query = @"
            UPDATE Doctor 
            SET Name = @Name, 
                Telephone = @Telephone, 
                Specialty = @Specialty, 
                IsActive = @IsActive 
            WHERE Id = @Id";

            using (var connection = _dbConnection.GetConnection())
            {
                try
                {
                    var affectedRows = await connection.ExecuteAsync(query, new
                    {
                        doctor.Name,
                        doctor.Telephone,
                        doctor.Specialty,
                        doctor.IsActive,
                        doctor.Id
                    });

                    if (affectedRows == 0)
                    {
                        throw new KeyNotFoundException("Médico não encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Erro a atualizar o médico.");
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

        public async Task<bool> HasConsultsAsync(int doctorId)
        {
            var query = "SELECT COUNT(1) FROM Consult WHERE Id_Doctor = @DoctorId";
            using (var connection = _dbConnection.GetConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(query, new { DoctorId = doctorId });
                return count > 0;
            }
        }

        //public async Task<Doctor> AuthenticationAsync(string email, string password)
        //{
        //    var doctor = await GetByEmailAsync(email);
        //    if (doctor == null)
        //    {
        //        return null;
        //    }
        //    if (!VerifyPassword(password, doctor.PasswordHash))
        //    {
        //        return null;
        //    }
        //    return doctor;
        //}
    }
}
