using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

public interface IAuthRepository
{

    Task<UserViewDTO?> FindByEmail(string email);
    string CreatePassword(string password);
    bool VerifyPassword(string password, string storedHash);
}
