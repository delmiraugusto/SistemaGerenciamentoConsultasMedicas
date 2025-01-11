namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class UpdateConsultDTO(
    int Id, 
    string? Description, 
    DateTime? DateTimeQuery, 
    bool? IsCanceled);

