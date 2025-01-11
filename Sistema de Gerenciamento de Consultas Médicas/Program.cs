using Sistema_de_Gerenciamento_de_Consultas_M�dicas.Application.ServiceApp;
using Sistema_de_Gerenciamento_de_Consultas_M�dicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_M�dicas.Domain.Infrastructure;
using Sistema_de_Gerenciamento_de_Consultas_M�dicas.Domain.IRepository;
using Sistema_de_Gerenciamento_de_Consultas_M�dicas.Domain.IService;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddScoped<IConsultService, ConsultService>();
builder.Services.AddScoped<IConsultRepository, ConsultRepository>();




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");

builder.Services.AddScoped<PostgresConnection>(provider =>
{
    return new PostgresConnection(connectionString);
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
