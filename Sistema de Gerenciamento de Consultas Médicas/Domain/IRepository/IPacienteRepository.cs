using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

public interface IPacienteRepository
{
    Task<IEnumerable<Paciente>> GetPacienteAsync();
    Task<Paciente> GetByIdAsync(int id);
    Task AddAsync(Paciente paciente);
    Task UpdateAsync(Paciente paciente);
    Task <bool> DeleteAsync(int id);
    
}
