
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

public interface IConsultService
{
    Task<IEnumerable<PatientConsultDTO>> GetConsultByPatientIdAsync(int idPatient);
    Task<IEnumerable<DoctorConsultDTO>> GetConsultByDoctorIdAsync(int idDoctor);
    Task<ConsultDTO> GetByIdAsync(int id);
    Task<int> AddAsync(Consult Consult);
    Task UpdateAsync(int id, ConsultDTO consultDTO);
    Task CancelAsync(int id);
}
