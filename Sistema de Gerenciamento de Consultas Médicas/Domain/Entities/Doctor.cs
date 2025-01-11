using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Doctor : Person
{
    public int Id { get; set; }
    public string Crm { get; set; }
    public bool IsActive { get; set; } = true;
    public override Role Funcao => Role.Medico;
    public string Specialty { get; set; }
    public Doctor() { }

    public Doctor(int id, string name, string email, string passwordHash, string telephone, string crm, string specialty)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Telephone = telephone;
        Crm = crm;
        Specialty = specialty;
    }
}
