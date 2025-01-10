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

    public async Task AddAsync(PatientDTO patientDTO)
    {
        var patient = new Patient
        {
            Name = patientDTO.Name,
            Email = patientDTO.Email,
            Age = patientDTO.Age,
            Telephone = patientDTO.Telephone
        };

        await _patientRepository.AddAsync(patient);
    }

    public async Task UpdateAsync(PatientDTO patientDTO)
    {
        var patient = await _patientRepository.GetByIdAsync(patientDTO.Id);

        if (patient == null)
        {
            throw new ApplicationException("Paciente não encontrado.");
        }

        patient.Name =  patientDTO.Name;
        patient.Email = patientDTO.Email;
        patient.Age = patientDTO.Age;
        patient.Telephone = patientDTO.Telephone;

        await _patientRepository.UpdateAsync(patient);
    }

    public async Task DeleteAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            throw new ApplicationException("Paciente não encontrado.");
        }

        await _patientRepository.DeleteAsync(id);
    }

    }
