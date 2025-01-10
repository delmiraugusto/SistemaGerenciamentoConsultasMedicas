using System.Data;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public abstract class Person
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Telephone { get; set; }
    public abstract Role Funcao { get; }
}
