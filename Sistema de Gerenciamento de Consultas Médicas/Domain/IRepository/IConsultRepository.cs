using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

public interface IConsultRepository
{
    Task<IEnumerable<Consult>> GetPatientAsync(int idPatient);
    Task<IEnumerable<Consult>> GetDoctorAsync(int idDoctor);
    Task<Consult> GetByIdAsync(int id);
    Task AddAsync(Consult consult);
    Task UpdateAsync(Consult consult);
    Task CancelAsync(int id);
}
