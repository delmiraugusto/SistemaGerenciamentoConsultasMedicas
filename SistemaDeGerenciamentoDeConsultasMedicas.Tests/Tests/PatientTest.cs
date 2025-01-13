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

public class PatientTest
{
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly PatientService _patientService;

    public PatientTest()
    {
        _mockPatientRepository = new Mock<IPatientRepository>();
        _patientService = new PatientService(_mockPatientRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_Test()
    {
        var patients = new List<Patient>
        {
            new Patient { Id = 1, Name = "Delso Silva", Email = "delso@gmail.com", Telephone = "123456789", Cpf = "123.456.789-00", Age = "30" },
            new Patient { Id = 2, Name = "Augusto Silva", Email = "augusto@gmail.com", Telephone = "987654321", Cpf = "987.654.321-00", Age = "28" }
        };

        _mockPatientRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(patients);

        var result = await _patientService.GetAllAsync();

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Test()
    {
        var patient = new Patient { Id = 1, Name = "Delso Silva", Email = "delso@gmail.com", Telephone = "123456789", Cpf = "123.456.789-00", Age = "30" };

        _mockPatientRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(patient);

        var result = await _patientService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByEmailAsync_Test()
    {
        var patient = new Patient { Id = 1, Name = "Delso Silva", Email = "delso@gmail.com", Telephone = "123456789", Cpf = "123.456.789-00", Age = "30" };

        _mockPatientRepository.Setup(repo => repo.GetByEmailAsync("delso@gmail.com")).ReturnsAsync(patient);

        var result = await _patientService.GetByEmailAsync("delso@gmail.com");

        Assert.NotNull(result);
        Assert.Equal("delso@gmail.com", result.Email);
    }


    [Fact]
    public async Task UpdateAsync_Test()
    {
        var patientDTO = new PatientDTO(
            Id: 1,
            Name: "Delso teste Up",
            Email: "delsoupdated@gmail.com",
            PasswordHash: "teste123",
            Cpf: "123.456.789-00",
            Age: "30",
            Telephone: "123456789"
        );

        var patient = new Patient
        {
            Id = 1,
            Name = "delso Silva",
            Email = "delso@gmail.com",
            Telephone = "123456789",
            Cpf = "123.456.789-00",
            Age = "30"
        };

        _mockPatientRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(patient);
        _mockPatientRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);

        await _patientService.UpdateAsync(1, patientDTO);

        _mockPatientRepository.Verify(repo => repo.UpdateAsync(It.Is<Patient>(p => p.Name == "Delso teste Up")), Times.Once);
    }


    [Fact]
    public async Task DeleteAsync_DeletePatient_Test()
    {
        var patient = new Patient { Id = 1, Name = "Delso Silva", Email = "delso@gmail.com", Telephone = "123456789", Cpf = "123.456.789-00", Age = "30" };

        _mockPatientRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(patient);
        _mockPatientRepository.Setup(repo => repo.HasConsultsAsync(1)).ReturnsAsync(false);
        _mockPatientRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _patientService.DeleteAsync(1);

        _mockPatientRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Exception_PatientHasConsults_Test()
    {
        var patient = new Patient { Id = 1, Name = "Delso Silva", Email = "delos@gmail.com", Telephone = "123456789", Cpf = "123.456.789-00", Age = "30" };

        _mockPatientRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(patient);
        _mockPatientRepository.Setup(repo => repo.HasConsultsAsync(1)).ReturnsAsync(true);

        await Assert.ThrowsAsync<ApplicationException>(() => _patientService.DeleteAsync(1));
    }


}
