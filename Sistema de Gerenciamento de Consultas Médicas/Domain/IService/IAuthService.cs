using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

public interface IAuthService
{   
    Task<int> Login(LoginDTO loginDTO);
    Task<int> signUp(SignUpDTO signUpDTO);


}
