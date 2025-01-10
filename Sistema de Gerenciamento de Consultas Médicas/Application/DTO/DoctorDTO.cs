using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class DoctorDTO(int Id, string Name, string Email, string Telephone, string Crm, Specialty Specialty, bool IsActive);