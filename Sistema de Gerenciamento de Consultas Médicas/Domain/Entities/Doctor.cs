using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Telephone { get; set; }
    public string Crm { get; set; }
    public bool IsActive { get; set; } = true;
    public Consult IdConsult { get; set; }

    public Specialty Specialty { get; set; }


    public Doctor() { }

    public Doctor(int id, string name, string email, string password, string telephone, string crm, Consult idConsult)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Telephone = telephone;
        Crm = crm;
        IdConsult = idConsult;
    }

}
