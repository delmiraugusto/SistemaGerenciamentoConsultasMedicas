using Microsoft.AspNetCore.Mvc;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ConsultController : ControllerBase
{   
    private readonly IConsultService _consultService;

    public ConsultController(IConsultService consultService)
    {
        _consultService = consultService;
    }

    [HttpGet("patientConsults/{pacienteId}")]
    public async Task<ActionResult<IEnumerable<ConsultDTO>>> GetAllPatient(int pacienteId)
    {
        try
        {
            var consults = await _consultService.GetConsultByPatientIdAsync(pacienteId);
            if (consults == null)
            {
                return NotFound($"Nenhuma consulta para o paciente com o Id: {pacienteId}.");
            }

            var formattedConsults = consults.Select(c => new
            {
                c.Id,
                c.Description,
                DateTimeQuery = c.DateTimeQuery.ToString("dd-MM-yyyy HH:mm:ss"),
                c.PatientId,
                c.PatientName,
                c.DoctorId,
                c.DoctorName,
                c.DoctorTelephone,
                c.DoctorSpecialty,
                c.IsCanceled
            });

            return Ok(formattedConsults);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Erro ao obter as consultas: {ex.Message}");
        }
    }


    [HttpGet("doctorConsults/{doctorId}")]
    public async Task<ActionResult<IEnumerable<ConsultDTO>>> GetAllDoctor(int doctorId)
    {
        try
        {
            var consults = await _consultService.GetConsultByDoctorIdAsync(doctorId);
            if (consults == null)
            {
                return NotFound($"Nenhuma consulta para o médico com o Id: {doctorId}.");
            }

            var formattedConsults = consults.Select(c => new
            {
                c.Id,
                c.Description,
                DateTimeQuery = c.DateTimeQuery.ToString("dd-MM-yyyy HH:mm:ss"),
                c.PatientId,
                c.PatientName,
                c.PatientAge,
                c.PatientTelephone,
                c.DoctorId,
                c.DoctorName,
                c.IsCanceled
            });

            return Ok(formattedConsults);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter as consultas: {ex.Message}");
        }
    }


}
