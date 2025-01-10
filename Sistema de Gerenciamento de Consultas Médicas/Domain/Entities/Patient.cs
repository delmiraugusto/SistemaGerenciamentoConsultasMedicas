using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Patient : Pessoa
{
    public int Id { get; set; }
    public string Age { get; set; }
    public override Role Funcao => Role.Paciente;
    public Patient() { }

    public Patient(int id, string name, string telephone, string age, string email, string passwordHash)
    {
        Id = id;
        Name = name;
        Telephone = telephone;
        Age = age;
        Email = email;
        PasswordHash = passwordHash;
    }

}
