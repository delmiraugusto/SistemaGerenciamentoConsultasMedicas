using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

namespace SistemaDeGerenciamentoDeConsultasMedicas.Tests.Tests;

public class DoctorTest
{

    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly DoctorService _doctorService;

    public DoctorTest()
    {
        _mockDoctorRepository = new Mock<IDoctorRepository>();
        _doctorService = new DoctorService(_mockDoctorRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_Test()
    {
        var doctors = new List<Doctor>
        {
            new Doctor { Id = 1, Name = "Dr. Delso", Email = "delso@gmail.com", Telephone = "123456789", Cpf = "123.456.789-00", Crm = "CRM123", PasswordHash = "passwordTest", Specialty = "Cardiologista", IsActive = true }
        };

        _mockDoctorRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(doctors);

        var result = await _doctorService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Dr. Delso", result.First().Name);
    }

    [Fact]
    public async Task GetByIdAsync_Test()
    {
        var doctor = new Doctor
        {
            Id = 1,
            Name = "Dr. Delso",
            Email = "delso@gmail.com",
            Telephone = "123456789",
            Cpf = "123.456.789-00",
            Crm = "CRM123",
            PasswordHash = "passwordTest",
            Specialty = "Cardiologista",
            IsActive = true
        };

        _mockDoctorRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(doctor); 

        var result = await _doctorService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Dr. Delso", result.Name);
    }

    [Fact]
    public async Task GetByEmailAsync_Test()
    {
        var doctor = new Doctor
        {
            Id = 1,
            Name = "Dr. João",
            Email = "delso@gmail.com",
            Telephone = "123456789",
            Cpf = "123.456.789-00",
            Crm = "CRM123",
            PasswordHash = "password",
            Specialty = "Cardiologista",
            IsActive = true
        };

        _mockDoctorRepository.Setup(repo => repo.GetByEmailAsync("delso@gmail.com"))
            .ReturnsAsync(doctor); 

        var result = await _doctorService.GetByEmailAsync("delso@gmail.com");

        Assert.NotNull(result);
        Assert.Equal("delso@gmail.com", result.Email);
    }

    [Fact]
    public async Task UpdateAsync_Test()
    {
        var doctorDTO = new Doctor
        {
            Id = 1,
            Name = "Dr. delso",
            Email = "delso@gmail.com",
            Telephone = "987654321",
            Cpf = "123.456.789-00",
            Crm = "CRM123",
            PasswordHash = "password",
            Specialty = "Cardiologista",
            IsActive = true
        };

        _mockDoctorRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(doctorDTO);

        _mockDoctorRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Doctor>()))
            .Returns(Task.CompletedTask);

        await _doctorService.UpdateAsync(1, doctorDTO);

        _mockDoctorRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Doctor>()), Times.Once);
    }

    [Fact]
    public async Task CancelAsync_DeactivateDoctor_WhenHasNoConsults_Test()
    {
        var doctor = new Doctor
        {
            Id = 1,
            Name = "Dr. Delso",
            Email = "delso@gmail.com",
            Telephone = "123456789",
            Cpf = "123.456.789-00",
            Crm = "CRM123",
            PasswordHash = "password",
            Specialty = "Cardiologista",
            IsActive = true
        };

        _mockDoctorRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        _mockDoctorRepository.Setup(repo => repo.HasConsultsAsync(1))
            .ReturnsAsync(false);

        _mockDoctorRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Doctor>()))
            .Returns(Task.CompletedTask);

        await _doctorService.CancelAsync(1);

        Assert.False(doctor.IsActive);
        _mockDoctorRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Doctor>()), Times.Once);
    }

    [Fact]
    public async Task CancelAsync_Exception_DoctorHasConsults_Test()
    {
        var doctor
            = new Doctor
        {
            Id = 1,
            Name = "Dr. Delso",
            Email = "delso@gmail.com",
            Telephone = "123456789",
            Cpf = "123.456.789-00",
            Crm = "CRM123",
            PasswordHash = "password",
            Specialty = "Cardiologista",
            IsActive = true
        };

        _mockDoctorRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        _mockDoctorRepository.Setup(repo => repo.HasConsultsAsync(1))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ApplicationException>(() => _doctorService.CancelAsync(1));
        Assert.Equal("Não é possível desativar o médico, pois ele está associado a uma ou mais consultas.", exception.Message);
    }

}
