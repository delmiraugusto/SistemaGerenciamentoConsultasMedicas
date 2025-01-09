using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Medico
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Telefone { get; set; }
    public string Crm { get; set; }
    public List<Consulta> Consultas { get; set; } = new();

    public bool Ativo { get; set; } 
    public Especialidade Especialidade { get; set; }

    public Endereco Endereco { get; set; }

    public Medico() { }

    public Medico(int id, string nome, string email, string senha, string telefone, string crm)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Senha = senha;
        Telefone = telefone;
        Crm = crm;
    }

    public void AddConsulta(Consulta consulta)
    {
        if (consulta != null && consulta.Medico == this)
        {
            Consultas.Add(consulta);
        }
        else
        {
            throw new ArgumentException("Consulta inválida ou o médico não corresponde.");
        }
    }
    public List<Consulta> HistoricoConsultas()
    {
        return Consultas;
    }

    public void excluir() {
        Ativo = false;
    }
}
