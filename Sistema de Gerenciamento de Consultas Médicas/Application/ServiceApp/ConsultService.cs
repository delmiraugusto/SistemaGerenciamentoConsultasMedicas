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

    public async Task AddAsync(ConsultDTO consultDTO)
    {
        var consult = new Consult
        {
            Id = consultDTO.Id,
            Description = consultDTO.Description,
            DateTimeQuery = consultDTO.DateTimeQuery,
            IdPatient = consultDTO.IdPatient,
            IdDoctor = consultDTO.IdDoctor,
            IsCanceled = consultDTO.IsCanceled,
        };
        await _consultRepository.AddAsync(consult);
    }


}
