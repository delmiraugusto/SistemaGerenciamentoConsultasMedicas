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
            doctor.Specialty,
            doctor.IsActive
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
            doctor.Specialty,
            doctor.IsActive
        );

    }

    public async Task<DoctorDTO> GetByIdAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

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
            doctor.Specialty,
            doctor.IsActive
        );
    }

    public async Task AddAsync(DoctorDTO doctorDTO)
    {
        var doctor = new Doctor
        {
            Name = doctorDTO.Name,
            Email = doctorDTO.Email,
            Specialty = doctorDTO.Specialty,
            IsActive = doctorDTO.IsActive
        };

        await _doctorRepository.AddAsync(doctor);
    }

    public async Task UpdateAsync(DoctorDTO doctorDTO)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorDTO.Id);

        if (doctor == null)
        {
            throw new ApplicationException("Médico não encontrado.");
        }

        doctor.Name = doctorDTO.Name;
        doctor.Email = doctorDTO.Email;
        doctor.Telephone = doctorDTO.Telephone;
        doctor.Crm = doctorDTO.Crm;
        doctor.Specialty = doctorDTO.Specialty;
        doctor.IsActive = doctorDTO.IsActive;

        await _doctorRepository.UpdateAsync(doctor);
    }

    public async Task CancelAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor == null)
        {
            throw new ApplicationException("Médico não encontrado para desativação.");
        }

        doctor.IsActive = false;

        await _doctorRepository.UpdateAsync(doctor);
    }
}
