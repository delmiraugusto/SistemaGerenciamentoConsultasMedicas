using System.Linq;
using System.Numerics;
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
        WHERE c.id_patient = @IdPatient AND c.IsCanceled = false
        ORDER BY c.DateTimeQuery;";

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
        WHERE d.id = @idDoctor
        ORDER BY c.DateTimeQuery;";


        using (var connection = _dbConnection.GetConnection())
        {
            var consults = await connection.QueryAsync<DoctorConsultDTO>(
                query,
                new { IdDoctor = idDoctor }

            );

            return consults;
        }
    }

    public async Task<ConsultDTO> GetByIdAsync(int id)
    {
        var query = @"
        SELECT 
            c.id AS Id, 
            c.description AS Description, 
            c.DateTimeQuery, 
            p.id AS IdPatient,
            p.Name AS PatientName,
            d.id AS IdDoctor,
            d.Name AS DoctorName,
            d.Telephone AS DoctorTelephone,
            d.specialty AS DoctorSpecialty,
            c.IsCanceled
        FROM Consult c
        JOIN Patient p ON c.id_patient = p.id
        JOIN Doctor d ON c.id_doctor = d.id
        WHERE c.id = @Id";

        using (var connection = _dbConnection.GetConnection())
        {
            var consult = await connection.QuerySingleOrDefaultAsync<ConsultDTO>(query, new { Id = id });
            return consult == null ? throw new KeyNotFoundException($"Consulta com o Id: {id} não encontrada") : consult;

        }
    }
    public async Task<Consult> AddAsync(Consult consult)
    {
        var query = @"
        INSERT INTO Consult (Description, DateTimeQuery, id_patient, id_doctor) 
        VALUES (@Description, @DateTimeQuery, @idPatient, @idDoctor)
        RETURNING Id";

        using (var connection = _dbConnection.GetConnection())
        {
            try
            {
                var newConsultId = await connection.ExecuteScalarAsync<int>(query, consult);

                consult.Id = newConsultId;
                return consult;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao adicionar a consulta." + ex.Message);
            }
        }
    }

    public async Task UpdateAsync(Consult consult)
    {
        var query = @"
        UPDATE Consult 
        SET 
            Description = @Description, 
            DateTimeQuery = @DateTimeQuery,
            IsCanceled = @IsCanceled
        WHERE Id = @Id";

        using (var connection = _dbConnection.GetConnection())
        {
            try
            {
                var affectedRows = await connection.ExecuteAsync(query, new
                {
                    consult.Id,
                    consult.Description,
                    consult.DateTimeQuery,
                    consult.IsCanceled,
                });

                if (affectedRows == 0)
                {
                    throw new KeyNotFoundException("Consulta não encontrada.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao atualizar a consulta: {ex.Message}");
            }
        }
    }

    public async Task CancelAsync(int id)
    {
        var query = "UPDATE Consult SET IsCanceled = TRUE WHERE id = @Id";
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
