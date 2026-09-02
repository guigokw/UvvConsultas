# Sistema de Gestão de Consultas UVV 

Sistema de gestão de consultas desenvolvido em **ASP.NET Core (C#)** com **Entity Framework Core** e **SQL Server**.

## Funcionalidades

- Cadastro de usuários;
- Login e autenticação;
- Registro, visualização, edição e exclusão de consultas.

Assista à [demonstração completa do sistema](https://www.loom.com/share/b48e879b8672481badf0797562421330).

## Instalação

### Pré-requisitos

- Visual Studio 2022
- SQL Server 

### Clonar o repositório

Abra o Visual Studio 2022 e selecione "Clonar um repositório". Cole a url do repositório:

   ```
   https://github.com/guigokw/UvvConsultas.git
   ```
Escolha o caminho do repositório e clique em "Clonar". O Visual Studio abrirá o projeto automaticamente.


### Restaurar os pacotes 

Abra o Console de Gerenciador de Pacotes (Ferramentas -> Gerenciador de Pacotes do Nuget -> Console do Gerenciador de Pacotes) e execute:
```bash
dotnet restore
```

### Criar a migration

No Console de Gerenciador de Pacotes, execute:

```bash
Add-Migration Initial
```

### Atualizar o banco de dados

No Console de Gerenciador de Pacotes, execute:

```bash
Update-Database
```

### Executar a aplicação

No Visual Studio, pressione F5 ou clique no botão "https".
A aplicação abrirá automaticamente no seu navegador.


## Licença

[MIT](https://choosealicense.com/licenses/mit/)