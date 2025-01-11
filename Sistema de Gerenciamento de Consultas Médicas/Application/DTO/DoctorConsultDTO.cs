using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class DoctorConsultDTO(
    int Id, 
    string Description, 
    DateTime DateTimeQuery, 
    int PatientId, 
    string PatientName, 
    string PatientAge, 
    string PatientTelephone, 
    int DoctorId, 
    string DoctorName, 
    bool IsCanceled);
