using System.Data;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure;

public class ConsultRepository : IConsultRepository
{
    private readonly PostgresConnection _dbConnection;
           
    public ConsultRepository(PostgresConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Consult>> GetPatientAsync(int idPatient)
    {
        var query = @"
    SELECT 
        c.id AS Id,
        c.description AS Description, 
        c.DateTimeQuery,  
        m.id AS DoctorId,
        m.Name AS DoctorName, 
        m.Speciality AS DoctorSpeciality
    FROM Consult c
    JOIN Doctor m ON c.id_doctor = m.id
    WHERE c.id_patient = @IdPatient AND c.IsCanceled = 0";

        using (var connection = _dbConnection.GetConnection())
        {
            var consults = await connection.QueryAsync<Consult, Doctor, Consult>(
                query,
                (consult, doctor) =>
                {
                    consult.IdDoctor = doctor;
                    return consult;
                },
                new { IdPatient = idPatient },
                splitOn: "DoctorId" 
            );

            return consults;
        }
    }

    public async Task<IEnumerable<Consult>> GetDoctorAsync(int idDoctor)
    {
        var query = @"
        SELECT 
            c.id AS Id, 
            c.description AS Description, 
            c.DateTimeQuery, 
            p.id AS PatientId, 
            p.Name AS PatientName,
            p.Email AS PatientEmail,
            m.id AS DoctorId, 
            m.Name AS DoctorName
        FROM Consult c
        JOIN Patient p ON c.id_patient = p.id
        JOIN Doctor m ON c.id_doctor = m.id
        WHERE c.id_doctor = @IdDoctor AND c.IsCanceled = 0";

        using (var connection = _dbConnection.GetConnection())
        {
            var consults = await connection.QueryAsync<Consult, Patient, Doctor, Consult>(
                query,
                (consult, patient, doctor) =>
                {
                    consult.IdPatient = patient;
                    consult.IdDoctor = doctor;
                    return consult;
                },
                new { IdDoctor = idDoctor },
                splitOn: "PatientId,DoctorId"
            );

            return consults;
        }
    }

    public async Task<Consult> GetByAsync(int id)
    {
        var query = "SELECT * FROM Consult WHERE id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            var consult = await connection.QueryFirstOrDefaultAsync<Consult>(query, new { Id = id });
            return consult == null ? throw new KeyNotFoundException($"Consulta com o ID {id} não foi encontrada.") : consult;
        }
    }

    public async Task AddAsync(Consult consult)
    {
            var query = "UPDATE Consult SET Description = @Description, DateTimeQuery = @DateTimeQuery WHERE Id = @Id";
            using (var connection = _dbConnection.GetConnection())
            {
                await connection.ExecuteAsync(query, consult);
            }
    }

    public async Task UpdateAsync(Consult consult)
    {
        var query = "INSERT INTO Consult (Description, DateTimeQuery, PatientId, DoctorId) VALUES (@Description, @DateTimeQuery, @PatientId, @DoctorId)";
        using (var connection = _dbConnection.GetConnection()) 
        {
            await connection.ExecuteAsync(query, consult);
        }
    }

    public async Task CancelAsync(int id)
    {
        var query = "UPDATE Consult SET IsCanceled = 1 WHERE id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Consulta não encontrado ou já foi cancelado.");
            }
        }
    }

}
