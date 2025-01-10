
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

public interface IConsultService
{
    Task<IEnumerable<ConsultDTO>> GetPatientAsync(int idPatient);
    Task<IEnumerable<ConsultDTO>> GetDoctorAsync(int idDoctor);
    Task<ConsultDTO> GetByIdAsync(int id);
    Task AddAsync(ConsultDTO consultDTO);
    Task UpdateAsync(int id, ConsultDTO consultDTO);
    Task CancelAsync(int id);
}
