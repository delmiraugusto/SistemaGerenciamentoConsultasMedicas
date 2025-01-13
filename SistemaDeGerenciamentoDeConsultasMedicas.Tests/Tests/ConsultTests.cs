using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

namespace SistemaDeGerenciamentoDeConsultasMedicas.Tests.Tests;

public class ConsultTests
{
    private readonly Mock<IConsultRepository> _mockConsultRepository;
    private readonly ConsultService _consultService;

    public ConsultTests()
    {
        _mockConsultRepository = new Mock<IConsultRepository>();
        _consultService = new ConsultService(_mockConsultRepository.Object);
    }

    [Fact]
    public async Task GetConsultByPatientIdAsync_Test()
    {
        var consults = new List<PatientConsultDTO>
       {
            new PatientConsultDTO(1, "Consulta Teste", DateTime.UtcNow, 1, "Paciente Delmir", 2, "Doutor Augusto", "123456789", "Cardiologista", false)
       };

        _mockConsultRepository
            .Setup(repo => repo.GetConsultByPatientIdAsync(It.IsAny<int>()))
            .ReturnsAsync(consults);

        var result = await _consultService.GetConsultByPatientIdAsync(1);

        Assert.NotNull(result);
        Assert.Single(result);
        _mockConsultRepository.Verify(repo => repo.GetConsultByPatientIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetConsultByDoctorIdAsync_Test()
    {
        var consults = new List<DoctorConsultDTO>
       {
            new DoctorConsultDTO(1, "Consulta Teset", DateTime.UtcNow, 1, "Paciente Delso", "30", "123456789", 2, "Doutor Silva", false)
       };

        _mockConsultRepository
            .Setup(repo => repo.GetDoctorAsync(It.IsAny<int>()))
            .ReturnsAsync(consults);

        var result = await _consultService.GetConsultByDoctorIdAsync(2);

        Assert.NotNull(result);
        Assert.Single(result);
        _mockConsultRepository.Verify(repo => repo.GetDoctorAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Test()
    {
        var consult = new ConsultDTO(
            1,
            "Consulta de rotina Test",
            DateTime.UtcNow,
            31,
            "Delmir Augusto",
            72,
            "Augusto Delmir",
            "123456789",
            "Dermatologista",
            false
        );

        _mockConsultRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(consult);

        var result = await _consultService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(consult.Id, result.Id);
        _mockConsultRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_Test()
    {
        var consult = new Consult
        {
            Description = "Consulta de rotina em Test",
            DateTimeQuery = DateTime.UtcNow.AddDays(1),
            IdPatient = 1,
            IdDoctor = 2
        };

        _mockConsultRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Consult>()))
            .ReturnsAsync(consult);

        var result = await _consultService.AddAsync(consult);

        Assert.Equal(consult.Id, result);
        _mockConsultRepository.Verify(repo => repo.AddAsync(It.IsAny<Consult>()), Times.Once);
    }



    [Fact]
    public async Task UpdateAsync_Test()
    {
        
        var existingConsultDTO = new ConsultDTO(
            1,
            "Consulta antiga Test",
            DateTime.UtcNow,
            1,
            "Augusto Delmir",
            2,
            "Silva Augusto",
            "123456789",
            "Dentista",
            false
        );

        var updateDto = new UpdateConsultDTO(
            1,
            "Consulta atualizada Test",
            DateTime.UtcNow.AddDays(1),
            true
        );

        _mockConsultRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(existingConsultDTO);

        _mockConsultRepository
            .Setup(repo => repo.UpdateAsync(It.IsAny<Consult>()))
            .Returns(Task.CompletedTask);

        await _consultService.UpdateAsync(1, updateDto);

        _mockConsultRepository.Verify(repo => repo.UpdateAsync(It.Is<Consult>(c =>
            c.Id == updateDto.Id &&
            c.Description == updateDto.Description &&
            c.DateTimeQuery == updateDto.DateTimeQuery &&
            c.IsCanceled == updateDto.IsCanceled)), Times.Once);
    }

    [Fact]
    public async Task CancelAsync_Test()
    {
        var consultDTO = new ConsultDTO(
            1,
            "Consulta cancelada Test",
            DateTime.UtcNow,
            1,
            "Delso da Silva",
            2,
            "Delmir Junior",
            "123456789",
            "Ortopedista",
            false
        );

        _mockConsultRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(consultDTO);  

        await _consultService.CancelAsync(1);

        _mockConsultRepository.Verify(repo => repo.CancelAsync(It.IsAny<int>()), Times.Once);
    }

}