using System.ComponentModel.DataAnnotations;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities
{
    public class Consulta
    {
        public int id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }

        public Consulta() { }

        public Consulta(string descricao, DateTime dataHoraConsulta, Paciente paciente, Medico medico)
        {
            Descricao = descricao;
            DataHoraConsulta = dataHoraConsulta;
            Paciente = paciente;
            Medico = medico;
        }

    }
}
