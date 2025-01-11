using System;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

public interface IConsultRepository
{
    Task<IEnumerable<PatientConsultDTO>> GetConsultByPatientIdAsync(int idPatient);
    Task<IEnumerable<DoctorConsultDTO>> GetDoctorAsync(int idDoctor);
    Task<ConsultDTO> GetByIdAsync(int id);
    Task<Consult> AddAsync(Consult consult);
    Task UpdateAsync(Consult consult);
    Task CancelAsync(int id);
}
