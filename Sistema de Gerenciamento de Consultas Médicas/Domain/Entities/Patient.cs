using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Patient : Person
{
    public int Id { get; set; }
    public string Age { get; set; }
    public string Cpf { get; set; }
    public override Role Funcao => Role.Paciente;
    public Patient() { }

    public Patient(int id, string name, string telephone, string age, string email, string passwordHash, string cpf)
    {
        Id = id;
        Name = name;
        Telephone = telephone;
        Age = age;
        Email = email;
        PasswordHash = passwordHash;
        Cpf = cpf;
    }

}
