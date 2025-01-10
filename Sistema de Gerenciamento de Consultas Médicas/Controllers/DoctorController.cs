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


}
