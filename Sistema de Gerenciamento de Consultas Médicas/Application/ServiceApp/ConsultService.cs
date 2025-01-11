using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;

public class ConsultService : IConsultService
{

    private readonly IConsultRepository _consultRepository;

    public ConsultService(IConsultRepository consultRepository)
    {
        _consultRepository = consultRepository;
    }

    public async Task<IEnumerable<PatientConsultDTO>> GetConsultByPatientIdAsync(int idPatient)
    {
        var consults = await _consultRepository.GetConsultByPatientIdAsync(idPatient);

        if (consults == null || !consults.Any())
        {
            throw new ApplicationException("Nenhuma consulta encontrada para esse paciente.");
        }

        return consults.Select(consult => new PatientConsultDTO(
            consult.Id,
            consult.Description,
            consult.DateTimeQuery,
            consult.PatientId,
            consult.PatientName,
            consult.DoctorId,
            consult.DoctorName,
            consult.DoctorTelephone,
            consult.DoctorSpecialty,
            consult.IsCanceled
        )).ToList();
    }

    public async Task<IEnumerable<DoctorConsultDTO>> GetConsultByDoctorIdAsync(int idDoctor)
    {
        var consults = await _consultRepository.GetDoctorAsync(idDoctor);

        if (consults == null || !consults.Any())
        {
            throw new ApplicationException("Nenhuma consulta encontrada para esse médico.");
        }

        return consults.Select(consult => new DoctorConsultDTO(
            consult.Id,
            consult.Description,
            consult.DateTimeQuery,
            consult.PatientId,
            consult.PatientName,
            consult.PatientAge,
            consult.PatientTelephone,
            consult.DoctorId,
            consult.DoctorName,
            consult.IsCanceled
        )).ToList();
    }

    public async Task<ConsultDTO> GetByIdAsync(int id)
    {
        var consult = await _consultRepository.GetByIdAsync(id);

        if (consult == null)
        {
            throw new ApplicationException("Consulta não encontrada");
        }

        return new ConsultDTO(
            consult.Id,
            consult.Description,
            consult.DateTimeQuery,
            consult.IdPatient.Id,
            consult.IdDoctor.Id,
            consult.IsCanceled
        );

    }


    public async Task AddAsync(ConsultDTO consultDTO)
    {
        var patient = new Patient { Id = consultDTO.IdPatient };
        var doctor = new Doctor { Id = consultDTO.IdDoctor };

        var consult = new Consult
        {
            Description = consultDTO.Description,
            DateTimeQuery = consultDTO.DateTimeQuery,
            IdPatient = patient,
            IdDoctor = doctor,
            IsCanceled = consultDTO.IsCanceled,
        };

        await _consultRepository.AddAsync(consult);
    }

    public async Task UpdateAsync(int id, ConsultDTO consultDTO)
    {
        var consult = await _consultRepository.GetByIdAsync(id);

        if (consult == null)
        {
            throw new ApplicationException("Consulta não encontrada");
        }

        var patient = new Patient { Id = consultDTO.IdPatient };
        var doctor = new Doctor { Id = consultDTO.IdDoctor };

        consult.IdDoctor = doctor;
        consult.Description = consultDTO.Description;
        consult.DateTimeQuery = consultDTO.DateTimeQuery;
        consult.IdPatient = patient;
        consult.IsCanceled = consultDTO.IsCanceled;

        await _consultRepository.UpdateAsync(consult);
    }

    public async Task CancelAsync(int id)
    {
        var consult = await _consultRepository.GetByIdAsync(id);
        if(consult == null)
        {
            throw new ApplicationException("Consulta não encontrada ou já desativada");
        }

        await _consultRepository.CancelAsync(id);
    }


}
