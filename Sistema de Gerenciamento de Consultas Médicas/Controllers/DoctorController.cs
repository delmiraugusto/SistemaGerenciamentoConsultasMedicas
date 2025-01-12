using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorController : ControllerBase
{

    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAllAsync()
    {
        try
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter os Médicos: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IsActiveDoctorDTO>> GetById(int id)
    {
        try
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound($"Médico com id: {id} não foi encontrado.");
            }
            return Ok(doctor);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter o Médico: {ex.Message}");
        }
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<DoctorDTO>> GetByEmail(string email)
    {
        try
        {
            var doctor = await _doctorService.GetByEmailAsync(email);
            if (doctor == null)
            {
                return NotFound($"Médico com o email: {email} não encontrado.");
            }
            return Ok(doctor);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter o Médico: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody] DoctorDTO doctor)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var id = await _doctorService.AddAsync(doctor);
            return CreatedAtAction(nameof(GetById), new { id }, doctor);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao adicionar o médico: {ex.Message}");
        }
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] DoctorDTO doctorDTO)
    {
        if (id != doctorDTO.Id)
        {
            return BadRequest("Id do médico no corpo da requisição difere do Id informado na URL.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _doctorService.UpdateAsync(id, doctorDTO);
            return Ok(doctorDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao atualizar o médico: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Cancel(int id)
    {
        try
        {
            await _doctorService.CancelAsync(id);
            return NoContent();
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao desativar o médico: {ex.Message}");
        }
    }


}
