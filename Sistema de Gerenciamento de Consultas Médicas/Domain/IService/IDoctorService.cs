using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDTO>> GetAllAsync();
    Task<DoctorDTO> GetByEmailAsync(string email);
    Task<DoctorDTO> GetByIdAsync(int id);
    Task<int> AddAsync(DoctorDTO doctorDTO);
    Task UpdateAsync(int id, DoctorDTO doctorDTO);
    Task CancelAsync(int id);
}
