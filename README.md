## Sistema de Agendamento de Consultas - C# & React VITE

## Sobre o Projeto

Crie uma API Web para gerenciar uma aplicação de sua escolha. Os usuários devem ser capazes de realizar as principais operações CRUD (Create, Read, Update, Delete) relacionadas ao tema escolhido. Além disso, o usuário deverá ter uma interface front-end para utilizar a aplicação.
</br>

### Critérios do Projeto

- Requisitos:
• .NET 8 ou superior: Utilize a última versão do .NET.
• React Vite
• DDD (Domain-Driven Design): Aplique os princípios de DDD para estruturar sua aplicação.
• Testes Unitários: Utilize NUnit ou xUnit para escrever testes unitários.
• Utilizar Dapper para acesso ao banco de dados
• .NET 8.0 ou superior
Bônus (Diferencial):
• Utilizar um tipo de autenticação. (basic, jwt e etc)
• Utilizar mensageria RabbitMQ ou outro.

### Estrutura do diretório do projeto:

```bash
/src
├── Sistema_de_Gerenciamento_de_Consultas_Médicas  # Solução principal
│   ├── Application/                 # Camada de aplicação
│   │  ├── DTO/                      # Objetos de transferência de dados (Data Transfer Objects)
│   │  │   ├── ConsultDTO.cs         # DTO para consultas
│   │  │   ├── DoctorConsultDTO.cs   # DTO para consultas de médicos
│   │  │   ├── DoctorDTO.cs          # DTO para médicos
│   │  │   ├── IsActiveDoctorDTO.cs  # DTO para validar médicos ativos
│   │  │   ├── LoginDTO.cs           # DTO para login
│   │  │   ├── PatientConsultDTO.cs  # DTO para consultas de pacientes
│   │  │   ├── SignUpDTO.cs          # DTO para registro de usuários
│   │  │   ├── UpdateConsultDTO.cs   # DTO para atualização de consultas
│   │  │   ├── UserViewDTO.cs        # DTO para visualização de usuários
│   │  ├── ServiceApp/               # Serviços de aplicação
│   │  │   ├── AuthService.cs        # Serviço de autenticação
│   │  │   ├── ConsultService.cs     # Serviço para consultas
│   │  │   ├── DoctorService.cs      # Serviço para médicos
│   │  │   ├── PatientService.cs     # Serviço para pacientes
│   │  ├── Data/                     # Camada de dados
│   │  │   ├── PostgresConnection.cs # Conexão com o banco PostgreSQL
│   ├── Domain/                      # Camada de domínio
│   │  ├── Entities/                 # Entidades do domínio
│   │  │   ├── Consult.cs            # Entidade de consultas
│   │  │   ├── Doctor.cs             # Entidade de médicos
│   │  │   ├── JwSettings.cs         # Configurações JWT
│   │  │   ├── Patient.cs            # Entidade de pacientes
│   │  │   ├── Person.cs             # Entidade base para pessoas
│   │  │   ├── Role.cs               # Entidade para papéis/roles
│   │  ├── IRepository/              # Interfaces para repositórios
│   │  │   ├── IAuthRepository.cs    # Interface para autenticação
│   │  │   ├── IConsultRepository.cs # Interface para consultas
│   │  │   ├── IDoctorRepository.cs  # Interface para médicos
│   │  │   ├── IPatientRepository.cs # Interface para pacientes
│   │  ├── IService/                 # Interfaces para serviços
│   │  │   ├── IAuthService.cs       # Interface para autenticação
│   │  │   ├── IConsultService.cs    # Interface para consultas
│   │  │   ├── IDoctorService.cs     # Interface para médicos
│   │  │   ├── IPatientService.cs    # Interface para pacientes
│   ├── Infrastructure/              # Implementações de repositórios
│   │  ├── AuthRepository.cs         # Repositório de autenticação
│   │  ├── ConsultRepository.cs      # Repositório de consultas
│   │  ├── DoctorRepository.cs       # Repositório de médicos
│   │  ├── PatientRepository.cs      # Repositório de pacientes
│   ├── RabbitMQ/                    # Configurações do RabbitMQ
│   │  ├── RabbitMQConsumer.cs       # Consumidor RabbitMQ
│   │  ├── RabbitMQPublisher.cs      # Publicador RabbitMQ
│   │  ├── RabbitMQSettings.cs       # Configurações do RabbitMQ
│   ├── Controllers/                 # Controladores para exposição dos endpoints
│   │  ├── AuthController.cs         # Controlador de autenticação
│   │  ├── ConsultController.cs      # Controlador de consultas
│   │  ├── DoctorController.cs       # Controlador de médicos
│   │  ├── PatientController.cs      # Controlador de pacientes
│   ├── appsettings.json             # Configurações gerais da aplicação
│   ├── Program.cs                   # Arquivo principal do programa
```

## 📁 Acesso ao projeto

**Acesse e baixar o código fonte do projeto final
[aqui](https://github.com/delmiraugusto/SistemaGerenciamentoConsultasMedicas).**


## Ferramentas utilizadas

- .NET 9.0
- Brcypt.Net
- Dapper
- JwtBearer
- ASP.NETCORE
- Npgsql
- RabbitMq
- XUNIT
