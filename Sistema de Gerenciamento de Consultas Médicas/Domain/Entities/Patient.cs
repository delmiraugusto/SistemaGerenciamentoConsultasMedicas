using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Telephone { get; set; }
    public string Age { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

   public Patient() { }

    public Patient(int id, string name, string telephone, string age, string email, string password)
    {
        Id = id;
        Name = name;
        Telephone = telephone;
        Age = age;
        Email = email;
        Password = password;
    }

    

}
