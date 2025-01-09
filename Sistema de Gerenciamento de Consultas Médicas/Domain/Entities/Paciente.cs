using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Paciente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Idade { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }

    public Paciente() {}

    public Paciente(int id, string nome, string telefone, string idade, string email, string senha, string cpf, ERole role)
    {
        Id = id;
        Nome = nome;
        Telefone = telefone;
        Idade = idade;
        Email = email;
        Senha = senha;
    }

}
