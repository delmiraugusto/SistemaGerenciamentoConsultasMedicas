namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class IsActiveDoctorDTO(
    int Id,
    string? Name,
    string? Email,
    string? Telephone,
    string? Crm,
    string? PasswordHash,
    string? Specialty,
    bool? IsActive);

