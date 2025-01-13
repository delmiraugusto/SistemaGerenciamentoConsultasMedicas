using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PostgresConnection _dbConnection;

        public PatientRepository(PostgresConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            var query = "SELECT * FROM Patient p " +
                        "ORDER BY p.NAME";
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
            var query = "SELECT * FROM Patient WHERE email = @Email";
            using (var connection = _dbConnection.GetConnection())
            {
                var patient = await connection.QueryFirstOrDefaultAsync<Patient>(query, new { Email = email });
                return patient == null ? throw new KeyNotFoundException($"Paciente não encontrado.") : patient;
            }
        }

        public async Task<Patient> AddAsync(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient), "O paciente não pode ser nulo.");
            }

            var query = "INSERT INTO Patient (Name, Email, PasswordHash, Telephone, Age, Cpf) " +
                        "VALUES (@Name, @Email, @PasswordHash, @Telephone, @Age, @Cpf)" +
                        "RETURNING Id";


            using (var connection = _dbConnection.GetConnection())
            {
                try
                {
                    var newPatientId = await connection.ExecuteScalarAsync<int>(query, patient);

                    patient.Id = newPatientId;
                    if (newPatientId <= 0)
                    {
                        throw new ApplicationException("Erro ao gerar o ID do paciente.");
                    }
                    return patient;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Erro ao adicionar o paciente." + ex.Message);
                }
            }
        }

        public async Task UpdateAsync(Patient patient)
        {
            var query = "UPDATE Patient SET Telephone = @Telephone, Age = @Age, Name = @Name WHERE id = @Id";
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

        public async Task<bool> HasConsultsAsync(int patientId)
        {
            var query = "SELECT COUNT(1) FROM Consult WHERE Id_Patient = @PatientId";
            using (var connection = _dbConnection.GetConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(query, new { PatientId = patientId });
                return count > 0;
            }
        }

        //public async Task<Patient> AuthenticationAsync(string email, string password)
        //{
        //    var patient = await GetByEmailAsync(email);
        //    if (patient == null)
        //    {
        //        return null;
        //    }
        //    if (!VerifyPassword(password, patient.PasswordHash))
        //    {
        //        return null;
        //    }
        //    return patient;
        //}
    }
}
