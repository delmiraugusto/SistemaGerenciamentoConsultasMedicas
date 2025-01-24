using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Presentation.Controllers;
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
    [Authorize]
    public async Task<ActionResult<IEnumerable<ConsultDTO>>> GetConsultByPatient(int pacienteId)
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Nenhuma consulta para o paciente com o Id: {ex.Message}");
        }
    }


    [HttpGet("doctorConsults/{doctorId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ConsultDTO>>> GetConsultByDoctor(int doctorId)
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

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ConsultDTO>>> GetById(int id)
    {
        try
        {
            var consult = await _consultService.GetByIdAsync(id);
            if (consult == null)
            {
                return NotFound($"Nenhuma consulta com o Id: {id}.");
            }

            var formattedConsult = new
            {
                consult.Id,
                consult.Description,
                DateTimeQuery = consult.DateTimeQuery.ToString("dd-MM-yyyy HH:mm:ss"),
                consult.IdPatient,
                consult.PatientName,
                consult.IdDoctor,
                consult.DoctorName,
                consult.DoctorTelephone,
                consult.DoctorSpecialty,
                consult.IsCanceled
            };

            return Ok(formattedConsult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter a consulta: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Add([FromBody] Consult consult)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _consultService.AddAsync(consult);
            return CreatedAtAction(nameof(GetById), new { consult.Id }, consult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao adicionar a consulta: {ex.Message}");
        }
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateConsultDTO consult)
    {
        if (id != consult.Id)
        {
            return BadRequest("Id da consulta no corpo da requisição difere do Id informado na URL.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _consultService.UpdateAsync(id, consult);
            return Ok(consult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao atualizar a consulta: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _consultService.CancelAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao cancelar a consulta: {ex.Message}");
        }
    }


}
