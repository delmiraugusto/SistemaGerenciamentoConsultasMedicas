namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class PatientDTO(
    int? Id, 
    string? Name, 
    string? Email,
    string? PasswordHash,
    string? Cpf,
    string? Age, 
    string? Telephone);