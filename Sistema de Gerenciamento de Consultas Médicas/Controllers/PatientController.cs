using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAllAsync()
    {
        try
        {
            var patiens = await _patientService.GetAllAsync();
            return Ok(patiens);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter os Pacientes: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDTO>> GetById(int id)
    {
        try
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound($"Paciente com id: {id} não foi encontrado.");
            }
            return Ok(patient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter o Paciente: {ex.Message}");
        }
    }

    [HttpGet("emailPatient/{email}")]
    public async Task<ActionResult<PatientDTO>> GetByEmail(string email)
    {
        try
        {
            var patient = await _patientService.GetByEmailAsync(email);
            if (patient == null)
            {
                return NotFound($"Paciente com o email: {email} não encontrado.");
            }
            return Ok(patient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter o Paciente: {ex.Message}");
        }
    }

    //[HttpPost]
    //public async Task<ActionResult> Add([FromBody] PatientDTO patient)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    try
    //    {
    //        var id = await _patientService.AddAsync(patient);

    //        return CreatedAtAction(nameof(GetById), new { id }, patient);
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Erro ao adicionar o Paciente: {ex.Message}");
    //    }
    //}

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] PatientDTO patientDTO)
    {
        if (id != patientDTO.Id)
        {
            return BadRequest("Id do paciente no corpo da requisição difere do Id informado na URL.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _patientService.UpdateAsync(id, patientDTO);
            return Ok(patientDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao atualizar o paciente: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _patientService.DeleteAsync(id);
            return NoContent();
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao excluir o usuário: {ex.Message}");
        }
    }


}
