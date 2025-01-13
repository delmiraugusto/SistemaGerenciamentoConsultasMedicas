namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string ExpirationMinutes { get; set; } = string.Empty;

}
