using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;

public class ConsultService : IConsultService
{

    private readonly IConsultRepository _consultRepository;
    private readonly RabbitMQPublisher _rabbitMQPublisher;

    public ConsultService(IConsultRepository consultRepository, RabbitMQPublisher rabbitMQPublisher)
    {
        _consultRepository = consultRepository;
        _rabbitMQPublisher = rabbitMQPublisher;
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
            consult.IdPatient,
            consult.PatientName,
            consult.IdDoctor,
            consult.DoctorName,
            consult.DoctorTelephone,
            consult.DoctorSpecialty,
            consult.IsCanceled
        );

    }

    public async Task<int> AddAsync(Consult consult)
    {

        if (string.IsNullOrEmpty(consult.Description))
            throw new ArgumentException("A descrição é obrigatória.");

        if (consult.DateTimeQuery == default(DateTime))
            throw new ArgumentException("A data da consulta é obrigatória.");

        if (consult.IdPatient <= 0)
            throw new ArgumentException("O ID do paciente é obrigatório e deve ser maior que 0.");

        if (consult.IdDoctor <= 0)
            throw new ArgumentException("O ID do médico é obrigatório e deve ser maior que 0.");

        var consultNew = new Consult
        {
            Description = consult.Description,
            DateTimeQuery = consult.DateTimeQuery,
            IdPatient = consult.IdPatient,
            IdDoctor = consult.IdDoctor,
            IsCanceled = consult.IsCanceled,
        };

        await _consultRepository.AddAsync(consultNew);
        var message = JsonSerializer.Serialize(consult);
        _rabbitMQPublisher.Publish(consult, "ConsultQueue");

        return consult.Id;
    }


    public async Task UpdateAsync(int id, UpdateConsultDTO consultDTO)
    {
        var editConsult = await _consultRepository.GetByIdAsync(id);

        if (editConsult == null)
        {
            throw new KeyNotFoundException("Consulta não encontrada");
        }

        var updatedConsult = new Consult
        {
            Id = editConsult.Id, 
            Description = consultDTO.Description ?? editConsult.Description,
            DateTimeQuery = consultDTO.DateTimeQuery ?? editConsult.DateTimeQuery,
            IsCanceled = consultDTO.IsCanceled ?? editConsult.IsCanceled
        };

        await _consultRepository.UpdateAsync(updatedConsult);

        _rabbitMQPublisher.Publish(updatedConsult, "ConsultUpdateQueue");

    }

    public async Task CancelAsync(int id)
    {
        var consult = await _consultRepository.GetByIdAsync(id);
        if(consult == null)
        {
            throw new ApplicationException("Consulta não encontrada ou já cancelada");
        }

        await _consultRepository.CancelAsync(id);
    }


}
