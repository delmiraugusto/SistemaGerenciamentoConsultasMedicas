namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class PatientConsultDTO(
    int Id,
    string Description,
    DateTime DateTimeQuery,
    int PatientId,
    string PatientName,
    int DoctorId,
    string DoctorName,
    string DoctorTelephone,
    string DoctorSpecialty,
    bool IsCanceled);
