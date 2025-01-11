namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class ConsultDTO(
        int Id,
        string Description,
        DateTime DateTimeQuery,
        int IdPatient,
        string PatientName,
        int IdDoctor,
        string DoctorName,
        string DoctorTelephone,
        string DoctorSpecialty,
        bool IsCanceled);

