using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;

public class PatientService : IPatientService
{

    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<PatientDTO>> GetAllAsync()
    {
        var patients = await _patientRepository.GetAllAsync();

        if (patients == null || !patients.Any())
        {
            throw new ApplicationException("Nenhum Paciente encontrado");
        }

        return patients.Select(patient => new PatientDTO(
                patient.Id,
                patient.Name,
                patient.Email,
                patient.PasswordHash,
                patient.Age,
                patient.Telephone
            )).ToList();
    }

    public async Task<PatientDTO> GetByEmailAsync(string email)
    {
        var patient = await _patientRepository.GetByEmailAsync(email);

        if (patient == null)
        {
            throw new ApplicationException("Nenhum Paciente encontrado");
        }

        return new PatientDTO(
            patient.Id,
            patient.Name,
            patient.Email,
            patient.PasswordHash,
            patient.Age,
            patient.Telephone
            );
    }

    public async Task<PatientDTO> GetByIdAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            throw new ApplicationException("Nenhum Paciente encontrado");
        }

        return new PatientDTO(
            patient.Id,
            patient.Name,
            patient.Email,
            patient.PasswordHash,
            patient.Age,
            patient.Telephone
            );
    }

    public async Task<int> AddAsync(PatientDTO patientDTO)
    {
        var patient = new Patient
        {
            Name = patientDTO.Name,
            Email = patientDTO.Email,
            PasswordHash = patientDTO.PasswordHash,
            Age = patientDTO.Age,
            Telephone = patientDTO.Telephone
        };

        await _patientRepository.AddAsync(patient);
        return patient.Id;
    }

    public async Task UpdateAsync(int id, PatientDTO patientDTO)
    {
        var patient = await _patientRepository.GetByIdAsync(patientDTO.Id);

        if (patient == null)
        {
            throw new ApplicationException("Paciente não encontrado.");
        }

        if (patientDTO.Id != id)
        {
            throw new ApplicationException("O ID não pode ser alterado.");
        }

        if (!string.IsNullOrEmpty(patientDTO.Email)) patient.Email = patientDTO.Email;
        if (!string.IsNullOrEmpty(patientDTO.Telephone)) patient.Telephone = patientDTO.Telephone;
        if (!string.IsNullOrEmpty(patientDTO.PasswordHash)) patient.PasswordHash = patientDTO.PasswordHash;
        if (!string.IsNullOrEmpty(patientDTO.Name)) patient.Name = patientDTO.Name;

        await _patientRepository.UpdateAsync(patient);
    }

    public async Task DeleteAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            throw new ApplicationException("Paciente não encontrado.");
        }

        var hasConsults = await _patientRepository.HasConsultsAsync(id);
        if (hasConsults)
        {
            throw new ApplicationException("Não é possível excluir o paciente pois ele está associado a uma consulta.");
        }

        await _patientRepository.DeleteAsync(id);
    }

}
