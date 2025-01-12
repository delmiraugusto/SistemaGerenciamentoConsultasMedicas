using System;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;



namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        { 
            var token = await _authService.Login(loginDTO);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao adicionar a consulta: {ex.Message}");
        }
    }

    [HttpPost("signUp")]
    public async Task<ActionResult> signUp([FromBody] SignUpDTO signUpDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var token = await _authService.signUp(signUpDTO);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao adicionar a consulta: {ex.Message}");
        }
    }


}
