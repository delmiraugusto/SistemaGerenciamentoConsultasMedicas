using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IRepository;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.IService;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Infrastructure;

namespace Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.ServiceApp;

public class AuthService : IAuthService
{

    private readonly IAuthRepository _authRepository;
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;

    public AuthService(IAuthRepository authRepository, IDoctorService doctorService, IPatientService patientService)
    {
        _authRepository = authRepository;
        _doctorService = doctorService;
        _patientService = patientService;
    }

    public async Task<int> Login(LoginDTO loginDTO)
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
            
        return user.id;

        //gerar o token e retornar ele sendo assim vai ficar Task<string> desse metodo de cima
        //o cadastro tambem deve retornar o JWT, criando uma funcao chamada GerarJWT, sendo chamando no final tanto do login e signup, nn vai mais retornar Id e sim token
        //criar funcao privada chamada gerarJwt, responsavel por fazer toda a geracao do JWT
    }

    public async Task<int> signUp(SignUpDTO signUpDTO)
    {

        if(signUpDTO.password.Length < 8)
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
                    DoctorDTO doctorDTO = new DoctorDTO(null, signUpDTO.name, signUpDTO.email, signUpDTO.telephone, signUpDTO.crm, passwordhash, signUpDTO.speciality);
                    idUserRegistered = await _doctorService.AddAsync(doctorDTO);
                    break;
                }
            case Role.Paciente:
                {
                    PatientDTO patientDTO = new PatientDTO(null, signUpDTO.name, signUpDTO.email, passwordhash, signUpDTO.age, signUpDTO.telephone);
                    idUserRegistered = await _patientService.AddAsync(patientDTO);
                    break;
                }
            default:
                {
                    throw new Exception("Role inválida! As roles permitidas são Paciente e Medico");
                    break;
                }
        }

        return idUserRegistered.Value;

    }

}
