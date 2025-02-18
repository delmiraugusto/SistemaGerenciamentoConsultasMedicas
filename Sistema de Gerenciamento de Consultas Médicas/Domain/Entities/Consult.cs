﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class Consult
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime DateTimeQuery { get; set; }
    [JsonPropertyName("id_patient")]
    public int IdPatient { get; set; }
    [JsonPropertyName("id_doctor")]

    public int IdDoctor { get; set; }
    public bool IsCanceled { get; set; } = false;

    public Consult() { }

    public Consult(int id, string description, DateTime dateTimeQuery, int idPatient, int idDoctor, bool isCanceled)
    {
        Id = id;
        Description = description;
        DateTimeQuery = dateTimeQuery;
        IdPatient = idPatient;
        IdDoctor = idDoctor;
        IsCanceled = isCanceled;
    }

}
