using System.Linq;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<IEnumerable<DoctorDTO>> GetAllAsync()
    {
        var doctors = await _doctorRepository.GetAllAsync();

        if (doctors == null || !doctors.Any())
        {
            throw new ApplicationException("Nenhum médico encontrado.");
        }

        return doctors.Select(doctor => new DoctorDTO(
            doctor.Id,
            doctor.Name,
            doctor.Email,
            doctor.Telephone,
            doctor.Crm,
            doctor.PasswordHash,
            doctor.Specialty
        )).ToList();
    }

    public async Task<DoctorDTO> GetByEmailAsync(string email)
    {
        var doctor = await _doctorRepository.GetByEmailAsync(email);

        if (doctor == null)
        {
            throw new ApplicationException("Médico não encontrado.");
        }

        return new DoctorDTO(
            doctor.Id,
            doctor.Name,
            doctor.Email,
            doctor.Telephone,
            doctor.Crm,
            doctor.PasswordHash,
            doctor.Specialty
        );

    }

    public async Task<IsActiveDoctorDTO> GetByIdAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor == null)
        {
            throw new ApplicationException("Médico não encontrado.");
        }

        return new IsActiveDoctorDTO(
            doctor.Id,
            doctor.Name,
            doctor.Email,
            doctor.Telephone,
            doctor.Crm,
            doctor.PasswordHash,
            doctor.Specialty,
            doctor.IsActive
        );
    }

    public async Task<int> AddAsync(DoctorDTO doctorDTO)
    {
        var doctor = new Doctor
        {
            Name = doctorDTO.Name,
            Email = doctorDTO.Email,
            Specialty = doctorDTO.Specialty,
            IsActive = true,
            PasswordHash = doctorDTO.PasswordHash,
            Telephone = doctorDTO.Telephone, 
            Crm = doctorDTO.Crm
        };

        await _doctorRepository.AddAsync(doctor);
        return doctor.Id;
    }

    public async Task UpdateAsync(int id, DoctorDTO doctorDTO)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor == null)
        {
            throw new ApplicationException("Médico não encontrado.");
        }

        if (doctorDTO.Id != id)
        {
            throw new ApplicationException("O ID não pode ser alterado.");
        }

        if (!string.IsNullOrEmpty(doctorDTO.Crm) && doctorDTO.Crm != doctor.Crm)
        {
            throw new ApplicationException("O CRM não pode ser alterado.");
        }

        if (!string.IsNullOrEmpty(doctorDTO.Name)) doctor.Name = doctorDTO.Name;
        if (!string.IsNullOrEmpty(doctorDTO.Email)) doctor.Email = doctorDTO.Email;
        if (!string.IsNullOrEmpty(doctorDTO.Telephone)) doctor.Telephone = doctorDTO.Telephone;
        if (!string.IsNullOrEmpty(doctorDTO.Specialty)) doctor.Specialty = doctorDTO.Specialty;
        if (!string.IsNullOrEmpty(doctorDTO.PasswordHash)) doctor.PasswordHash = doctorDTO.PasswordHash;

        await _doctorRepository.UpdateAsync(doctor);
    }

    public async Task CancelAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor == null)
        {
            throw new ApplicationException("Médico não encontrado para desativação.");
        }

        var hasConsults = await _doctorRepository.HasConsultsAsync(id);
        if (hasConsults)
        {
            throw new ApplicationException("Não é possível desativar o médico, pois ele está associado a uma ou mais consultas.");
        }

        doctor.IsActive = false;

        await _doctorRepository.UpdateAsync(doctor);
    }
}
