using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class SignUpDTO(
    Role role, 
    string name, 
    string email, 
    string password,
    string telephone,
    string cpf,
    string? crm,
    string? specialty,
    string? age);

