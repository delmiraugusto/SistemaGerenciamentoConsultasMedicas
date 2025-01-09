using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<Medico>> GetMedicoAsync();
        Task<Medico> GetByIdAsync(int id);
        Task AddAsync(Medico medico);
        Task UpdateAsync(Medico medico);
        Task <bool> DeleteAsync(int id);

    }
}