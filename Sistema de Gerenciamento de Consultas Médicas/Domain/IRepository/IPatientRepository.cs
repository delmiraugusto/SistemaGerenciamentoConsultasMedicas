﻿using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient> GetByEmailAsync(string email);
    Task<Patient> GetByIdAsync(int id);
    Task<Patient> AddAsync(Patient patient);
    Task UpdateAsync(Patient patient);
    Task DeleteAsync(int id);
    Task<bool> HasConsultsAsync(int patientId);
    //Task<Patient> AuthenticationAsync(string email, string password);

}
