#  Sistema de Gestão de Consultas UVV

##  Sobre o Projeto

Sistema web desenvolvido em **ASP.NET Core MVC** para gerenciamento de consultas médicas. A aplicação permite o cadastro de usuários, autenticação, e o registro completo de consultas, aplicando conceitos de persistência de dados, separação de preocupações (SoC) e segurança.

---

## Tecnologias Utilizadas

| Tecnologia | Descrição |
|------------|-----------|
| ASP.NET Core MVC | Framework para desenvolvimento web |
| .NET 8 | Plataforma de desenvolvimento |
| Entity Framework Core | ORM para acesso a dados (Code First) |
| SQL Server LocalDB | Banco de dados relacional |
| Bootstrap 5 | Estilização da interface |
| Autenticação via Cookies | Controle de acesso e login |


## Como Executar o Projeto

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (com carga de trabalho ASP.NET)
- SQL Server LocalDB (instalado com o Visual Studio)

### Passos para execução

1. **Clone o repositório**

```bash

git clone https://github.com/guigokw/UvvConsultas.git
cd UvvConsultas

```

2. **Restaure os pacotes**

```bash
dotnet restore

```

3. **Compile o projeto**

```bash
dotnet build
```

---

### Configuração do Banco de Dados

O projeto utiliza Entity Framework Core com abordagem Code First e banco de dados SQL Server LocalDB.

- String de Conexão:

A string de conexão está configurada no arquivo appsettings.json:

```bash
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=UVVConsultasDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

- Criar e aplicar as migrações:

Com o projeto aberto no Visual Studio, abra o Package Manager Console (Ferramentas → Gerenciador de Pacotes NuGet → Console do Gerenciador de Pacotes) e execute:

```bash
Add-Migration Initial
```

```bash
Update-Database
```

Ou via terminal (fora do Visual Studio):

```bash
dotnet ef migrations add Initial
dotnet ef database update
```

### Verificar se o banco foi criado
1. No Visual Studio, vá em Exibir → SQL Server Object Explorer.

2. Expanda (localdb)\MSSQLLocalDB → Bancos de Dados.

3. O banco UVVConsultasDb deve aparecer com as tabelas:

- Usuarios

- Consultas

- __EFMigrationsHistory

▶### Executar a Aplicação
```bash
dotnet run
```

ou aperte o ícone de iniciar na parte de cima da tela do visual studio

Acesse no navegador:

text
https://localhost:5001
(A porta pode variar – verifique no terminal após o dotnet run)
