using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

public interface IPatientService
{
    Task<IEnumerable<PatientDTO>> GetAllAsync();
    Task<PatientDTO> GetByEmailAsync(string email);
    Task<PatientDTO> GetByIdAsync(int id);
    Task<int> AddAsync(PatientDTO patientDTO);
    Task UpdateAsync(int id, PatientDTO patientDTO);
    Task DeleteAsync(int id);
}
