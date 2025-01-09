using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

public interface IConsultaRepository
{
    Task<IEnumerable<Consulta>> GetPacienteAsync(int idPaciente);
    Task<IEnumerable<Consulta>> GetMedicoAsync(int idMedico);
    Task<Consulta> GetByAsync(int id);
    Task AddAsync(Consulta consulta);
    Task UpdateAsync(Consulta consulta);
    Task <bool> DeleteAsync(int id);
}
