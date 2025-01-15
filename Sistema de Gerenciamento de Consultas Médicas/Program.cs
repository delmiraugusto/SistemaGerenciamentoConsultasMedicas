using System.Text.Json.Serialization;
using System.Text.Json;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Data;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Infrastructure;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Infrastructure;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.RabbitMQ;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddScoped<IConsultService, ConsultService>();
builder.Services.AddScoped<IConsultRepository, ConsultRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.Configure<RabbitMQSettings>(Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddHostedService<RabbitMQConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
            {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
        }
    });


});

var jwtSecret = builder.Configuration["Jwt:Secret"];
var jwtExpiration = builder.Configuration["Jwt:ExpirationMinutes"];

if (string.IsNullOrEmpty(jwtSecret))
{
    throw new InvalidOperationException("A chave secreta do JWT não foi configurada corretamente.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret))
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyMethod() 
              .AllowAnyHeader()
              .AllowCredentials(); 
    });
});

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");

builder.Services.AddScoped<PostgresConnection>(provider =>
{
    return new PostgresConnection(connectionString);
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
