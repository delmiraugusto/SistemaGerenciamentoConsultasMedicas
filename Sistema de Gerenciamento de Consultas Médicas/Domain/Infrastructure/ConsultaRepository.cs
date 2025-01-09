using System.Data;
using Dapper;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure;

public class ConsultaRepository : IConsultaRepository
{
    private readonly IDbConnection _dbConnection;
            
    public ConsultaRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Consulta>> GetPacienteAsync(int idPaciente)
    {
        var query = "SELECT * FROM Consulta WHERE id_paciente = @IdPaciente";
        return await _dbConnection.QueryAsync<Consulta>(query, new { IdPaciente = idPaciente });
    }
    public async Task<IEnumerable<Consulta>> GetMedicoAsync(int idMedico)
    {
        var query = "SELECT * FROM Consulta WHERE id_medico = @IdMedico";
        return await _dbConnection.QueryAsync<Consulta>(query, new { IdMedico = idMedico });
    }

    //public async Task<Consulta> GetByAsync(int id)
    //{
    //    var query = "SELECT * FROM Consulta WHERE id = @Id";
    //    return await _dbConnection.QueryFirstOrDefaultAsync<Consulta>(query, new { Id = id });
    //}

    //public async Task AddAsync(Consulta consulta)
    //{
    //    var query = @"
    //    INSERT INTO Consulta (Descricao, DataHoraConsulta, id_paciente, id_medico)
    //    VALUES (@Descricao, @DataHoraConsulta, @IdPaciente, @IdMedico)";

    //    await _dbConnection.ExecuteAsync(query, new
    //    {
    //        consulta.Descricao,
    //        consulta.DataHoraConsulta,
    //        IdPaciente = consulta.Paciente?.id,
    //        IdMedico = consulta.Medico?.Id
    //    });
    //}

    //public async Task UpdateAsync(Consulta consulta)
    //{
    //    var query = @"
    //    UPDATE Consulta
    //    SET Descricao = @Descricao,
    //        DataHoraConsulta = @DataHoraConsulta,
    //        id_paciente = @IdPaciente,
    //        id_medico = @IdMedico
    //    WHERE id = @Id";

    //    await _dbConnection.ExecuteAsync(query, new
    //    {
    //        consulta.Descricao,
    //        consulta.DataHoraConsulta,
    //        IdPaciente = consulta.Paciente?.id,
    //        IdMedico = consulta.Medico?.Id,
    //        consulta.id
    //    });
    //}

    //public async Task<bool> DeleteAsync(int id)
    //{
    //    var query = "DELETE FROM Consulta WHERE id = @Id";
    //    var rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id });
    //    return rowsAffected > 0;
    //}

}
