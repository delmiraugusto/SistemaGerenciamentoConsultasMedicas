using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;

public record class PatientConsultDTO(int Id , string Description, DateTime DateTimeQuery, int IdPatient, string NamePatient, int IdDoctor, string NameDoctor, string TelephoneDoctor, string Speciality, bool IsCanceled);