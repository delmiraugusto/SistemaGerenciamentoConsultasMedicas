using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Consult
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime DateTimeQuery { get; set; }
    public Patient IdPatient { get; set; }
    public Doctor IdDoctor { get; set; }
    public bool IsCanceled { get; set; } = false;

    public Consult() { }

    public Consult(int id, string description, DateTime dateTimeQuery, Patient idPatient, Doctor idDoctor)
    {
        Id = id;
        Description = description;
        DateTimeQuery = dateTimeQuery;
        IdPatient = idPatient;
        IdDoctor = idDoctor;
    }

}
