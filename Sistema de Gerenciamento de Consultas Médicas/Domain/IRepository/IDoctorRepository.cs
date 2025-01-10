using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByEmailAsync(string email);
        Task<Doctor> GetByIdAsync(int id);
        Task AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
        Task CancelAsync(int id);
        Task<Doctor> AuthenticationAsync(string email, string password);

    }
}