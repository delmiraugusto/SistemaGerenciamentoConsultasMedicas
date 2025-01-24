using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Presentation.Controllers;

namespace SistemaDeGerenciamentoDeConsultasMedicas.Tests.Tests;

public class AuthTest
{

    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;

    public AuthTest()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }


    [Fact]
    public async Task Login()
    {

        var loginDTO = new LoginDTO("delso@gmail.com", "Delso123");
        var expectedToken = "teste-generation-jwt-token";
        _mockAuthService.Setup(service => service.Login(It.IsAny<LoginDTO>())).ReturnsAsync(expectedToken);

        var result = await _controller.Login(loginDTO);

        var actionResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedToken, actionResult.Value);
    }

    [Fact]
    public async Task SignUpPatientTest()
    {
        var signUpDTO = new SignUpDTO(
            role: Role.Paciente,      
            name: "Delmir Augusto",        
            email: "delmir@gmail.com",
            password: "Delso123",  
            telephone: "1234567890",
            cpf: "12345678901",
            crm: null, 
            specialty: null,
            age: "30" 
        );

        var expectedId = 1;

        _mockAuthService.Setup(service => service.signUp(It.IsAny<SignUpDTO>())).ReturnsAsync(expectedId);

        var result = await _controller.signUp(signUpDTO);

        var actionResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedId, actionResult.Value);
    }

    [Fact]
    public async Task SignUpDoctorTest()
    {
        
        var signUpDTO = new SignUpDTO(
            role: Role.Medico,          
            name: "Dr. Augusto",           
            email: "Augusto@example.com",
            password: "Password123",
            telephone: "9876543210",    
            cpf: "98765432109",        
            crm: "123456",             
            specialty: "Cardiologista",    
            age: null                   
        );

        var expectedId = 1;

        _mockAuthService.Setup(service => service.signUp(It.IsAny<SignUpDTO>())).ReturnsAsync(expectedId);

        var result = await _controller.signUp(signUpDTO);

        var actionResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedId, actionResult.Value); 
    }







}
