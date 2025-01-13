﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Infrastructure;
using System.Xml.Linq;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;

public class AuthService : IAuthService
{

    private readonly IAuthRepository _authRepository;
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly string _jwtsecret;
    private readonly string _jwtExpirationMinutes;

    public AuthService(IAuthRepository authRepository, IDoctorService doctorService, IPatientService patientService)
    {
        _authRepository = authRepository;
        _doctorService = doctorService;
        _patientService = patientService;
    }

    public async Task<string> Login(LoginDTO loginDTO) 
    {
        var user = await _authRepository.FindByEmail(loginDTO.email);

        if (user == null) 
        {
            throw new Exception("Email nao encontrado");
        }


        if (!_authRepository.VerifyPassword(loginDTO.password, user.passwordhash))
        {
            throw new Exception("Dados incorretos! Tente Novamente");
        }

        if (!Enum.TryParse<Role>(user.role, true, out var roleEnum))
        {
            throw new Exception($"Role inválido: {user.role}");
        }

        return GenerateJwtToken(user.name, roleEnum);

        //gerar o token e retornar ele sendo assim vai ficar Task<string> desse metodo de cima
        //o cadastro tambem deve retornar o JWT, criando uma funcao chamada GerarJWT, sendo chamando no final tanto do login e signup, nn vai mais retornar Id e sim token
    }

    public async Task<int> signUp(SignUpDTO signUpDTO)
    {


        if (string.IsNullOrWhiteSpace(signUpDTO.password) || signUpDTO.password.Length < 8)
        {
            throw new Exception("Senha deve ter no mínimo 8 caracteres");
        }

        var user = await _authRepository.FindByEmail(signUpDTO.email);

        if (user != null)
        {
            throw new Exception("Email já registrado por outro usuário");
        }

        var passwordhash = _authRepository.CreatePassword(signUpDTO.password);
        int? idUserRegistered = null;

        switch (signUpDTO.role)
        {
            case Role.Medico : 
                {
                    DoctorDTO doctorDTO = new DoctorDTO(null, signUpDTO.name, signUpDTO.email, signUpDTO.telephone, signUpDTO.cpf, signUpDTO.crm, passwordhash, signUpDTO.specialty);
                    idUserRegistered = await _doctorService.AddAsync(doctorDTO);
                    break;
                }
            case Role.Paciente:
                {
                    PatientDTO patientDTO = new PatientDTO(null, signUpDTO.name, signUpDTO.email, passwordhash, signUpDTO.cpf, signUpDTO.age, signUpDTO.telephone);
                    idUserRegistered = await _patientService.AddAsync(patientDTO);
                    break;
                }
            default:
                {
                    throw new Exception("Role inválida! As roles permitidas são Paciente e Medico");
                    break; string name;

                }
        }

        //Console.WriteLine($"Valor recebido para role: {signUpDTO.role}");


        //if (!Enum.TryParse<Role>(signUpDTO.role.ToString(), true, out var roleEnum))
        //{
        //    throw new Exception($"Role inválida: {signUpDTO.role}");
        //}

        return idUserRegistered.Value;
        //return "teste";

    }

    private string GenerateJwtToken(string name, Role role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtsecret);
        if (!int.TryParse(_jwtExpirationMinutes, out var expirationMinutes))
        {
            throw new InvalidOperationException("JWT ExpirationMinutes is not a valid integer");
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    //new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, role.ToString()),
                }),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
