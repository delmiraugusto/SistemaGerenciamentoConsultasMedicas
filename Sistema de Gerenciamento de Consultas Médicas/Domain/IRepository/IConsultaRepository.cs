using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

public interface IConsultaRepository
{
    Task<IEnumerable<Consult>> GetPatientAsync(int idPatient);
    Task<IEnumerable<Consult>> GetDoctorAsync(int idDoctor);
    Task<Consult> GetByAsync(int id);
    Task AddConsultOrUpdateAsync(Consult consult);
    Task CancelAsync(int id);
}
