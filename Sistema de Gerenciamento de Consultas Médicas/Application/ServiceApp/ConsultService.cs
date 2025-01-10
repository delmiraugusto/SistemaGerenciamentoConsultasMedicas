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

    public async Task<IEnumerable<ConsultDTO>> GetPatientAsync(int idPatient)
    {
        var consults = await _consultRepository.GetPatientAsync(idPatient);

        if (consults == null || !consults.Any())
        {
            throw new ApplicationException("Nenhuma consulta encontrada para esse paciente.");
        }

        return consults.Select(consult => new ConsultDTO(
            consult.Id,
            consult.Description,
            consult.DateTimeQuery,
            consult.IdPatient,
            consult.IdDoctor,
            consult.IsCanceled
        )).ToList();
    }


    public async Task<IEnumerable<ConsultDTO>> GetDoctorAsync(int idDoctor)
    {
        var consults = await _consultRepository.GetDoctorAsync(idDoctor);

        if (consults == null || !consults.Any())
        {
            throw new ApplicationException("Nenhuma consulta encontrada para esse médico.");
        }

        return consults.Select(consult => new ConsultDTO(
            consult.Id,
            consult.Description,
            consult.DateTimeQuery,
            consult.IdPatient,
            consult.IdDoctor,
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
            consult.IdPatient,
            consult.IdDoctor,
            consult.IsCanceled
        );

    }


    public async Task AddAsync(ConsultDTO consultDTO)
    {
        var consult = new Consult
        {
            Description = consultDTO.Description,
            DateTimeQuery = consultDTO.DateTimeQuery,
            IdPatient = consultDTO.IdPatient,
            IdDoctor = consultDTO.IdDoctor,
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

        consult.IdDoctor = consultDTO.IdDoctor;
        consult.Description = consultDTO.Description;
        consult.DateTimeQuery = consultDTO.DateTimeQuery;
        consult.IdPatient = consultDTO.IdPatient;
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
