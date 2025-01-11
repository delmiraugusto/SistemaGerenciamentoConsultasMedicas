using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
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

    public async Task<IEnumerable<PatientConsultDTO>> GetConsultByPatientIdAsync(int idPatient)
    {
        var query = @"
        SELECT 
            c.id AS Id,
            c.description AS Description, 
            c.DateTimeQuery,  
            p.id AS PatientId,
            p.Name AS PatientName,
            d.id AS DoctorId,
            d.Name AS DoctorName,
            d.Telephone AS DoctorTelephone,
            d.specialty AS DoctorSpecialty,
            c.IsCanceled
        FROM Consult c
        JOIN Doctor d ON c.id_doctor = d.id
        JOIN Patient p ON c.id_patient = p.id
        WHERE c.id_patient = @IdPatient AND c.IsCanceled = false;";

        using (var connection = _dbConnection.GetConnection())
        {
            var consults = await connection.QueryAsync<PatientConsultDTO>(
                query,
                new { IdPatient = idPatient }
            );

            return consults;
        }
    }


    public async Task<IEnumerable<DoctorConsultDTO>> GetDoctorAsync(int idDoctor)
    {
        var query = @"
        SELECT 
            c.id AS Id, 
            c.description AS Description, 
            c.DateTimeQuery, 
            p.id AS PatientId, 
            p.Name AS PatientName,
            p.Age AS PatientAge, 
            p.Telephone AS PatientTelephone,
            d.id AS DoctorId, 
            d.Name AS DoctorName,
            c.IsCanceled AS IsCanceled
        FROM Consult c
        JOIN Patient p ON c.id_patient = p.id
        JOIN Doctor d ON c.id_doctor = d.id
        WHERE d.id = @idDoctor AND c.IsCanceled = false;";


        using (var connection = _dbConnection.GetConnection())
        {
            var consults = await connection.QueryAsync<DoctorConsultDTO>(
                query,
                new { IdDoctor = idDoctor }

            );

            return consults;
        }
    }


    public async Task<Consult> GetByIdAsync(int id)
    {
        var query = @"
        SELECT 
            c.id AS Id, 
            c.description AS Description, 
            c.DateTimeQuery, 
            c.IsCanceled,
            p.id AS PatientId, 
            p.Name AS PatientName,
            m.id AS DoctorId, 
            m.Name AS DoctorName,
            m.specialty AS DoctorSpecialty
        FROM Consult c
        JOIN Patient p ON c.id_patient = p.id
        JOIN Doctor m ON c.id_doctor = m.id
        WHERE c.id = @Id";

        using (var connection = _dbConnection.GetConnection())
        {
            var consult = await connection.QueryAsync<Consult, Patient, Doctor, Consult>(
                query,
                (consult, patient, doctor) =>
                {
                    consult.IdPatient = patient;
                    consult.IdDoctor = doctor;
                    return consult;
                },
                new { Id = id },
                splitOn: "PatientId,DoctorId"
            );

            return consult.FirstOrDefault() ?? throw new KeyNotFoundException($"Consulta com o ID {id} não foi encontrada.");
        }
    }


    public async Task UpdateAsync(Consult consult)
    {
        var query = "UPDATE Consult SET Description = @Description, DateTimeQuery = @DateTimeQuery WHERE Id = @Id";
        using (var connection = _dbConnection.GetConnection())
        {
            await connection.ExecuteAsync(query, consult);
        }
    }

    public async Task AddAsync(Consult consult)
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
