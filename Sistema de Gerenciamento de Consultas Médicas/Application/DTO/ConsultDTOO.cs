using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class ConsultDTO(
            int Id,
            string Description,
            DateTime DateTimeQuery,
            int IdPatient,
            int IdDoctor,
            bool IsCanceled);

