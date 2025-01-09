namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Endereco
{
    public string Logradouro { get; set; }
    public string Bairro { get; set; }
    public string Cep { get; set; }
    public string Numero { get; set; }
    public string Complemento { get; set; } 
    public string Cidade { get; set; }
    public string Uf { get; set; }

    public Endereco() { }

    public Endereco(string logradouro, string bairro, string cep, string numero, string complemento, string cidade, string uf)
    {
        Logradouro = logradouro;
        Bairro = bairro;
        Cep = cep;
        Numero = numero;
        Complemento = complemento;
        Cidade = cidade;
        Uf = uf;
    }
}
