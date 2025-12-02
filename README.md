# ?? FCG API - Sistema de Gerenciamento de Jogos

API RESTful para gerenciamento de jogos e usuários, desenvolvida com .NET 8, Clean Architecture e autenticação JWT.

## ?? Tecnologias

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **FluentValidation**
- **MediatR (CQRS)**
- **xUnit + Moq (Testes)**
- **Swagger/OpenAPI**

## ?? Estrutura do Projeto
```
Fcg/
??? Fcg.Api/              # Camada de apresentação (Controllers/Endpoints)
??? Fcg.Domain/           # Lógica de negócio (Handlers, Entities, Interfaces)
??? Fcg.Data/             # Acesso a dados (Repositórios, DbContext)
??? Fcg.Shareable/        # DTOs, Requests, Responses
??? Fcg.IOC/              # Injeção de dependências
??? Fcg.Tests/            # Testes unitários
```

## ?? Pré-requisitos

- [.NET 8 SDK]
- [SQL Server](ou SQL Server Express)
- [Visual Studio 2022] ou [VS Code]

## ?? Configuração

### 1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/fcg-api.git
cd fcg-api
```

### 2. Configure a connection string

Crie o arquivo `appsettings.Development.json` em `Fcg.Api/`:
```json
{
  "ConnectionStrings": {
    "ConnectionString": "Server=localhost;Database=FcgDb;User Id=sa;Password=SuaSenha;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "SuaChaveSecretaComPeloMenos32Caracteres!",
    "Issuer": "FcgApi",
    "Audience": "FcgApi",
    "ExpireMinutes": 1440
  }
}
```

**Ou use User Secrets (recomendado):**
```bash
cd Fcg.Api
dotnet user-secrets set "ConnectionStrings:ConnectionString" "Server=localhost;Database=FcgDb;User Id=sa;Password=SuaSenha;TrustServerCertificate=True"
dotnet user-secrets set "Jwt:Key" "SuaChaveSecretaComPeloMenos32Caracteres!"
```

### 3. Execute as migrations
```bash
cd Fcg.Data
dotnet ef database update --startup-project ../Fcg.Api
```

### 4. Execute o projeto
```bash
cd Fcg.Api
dotnet run
```

A API estará disponível em: `https://localhost:5001` ou `http://localhost:5000`

Acesse o Swagger: `https://localhost:5001/swagger`

## ?? Executar Testes
```bash
dotnet test
```

## ?? Endpoints principais

### Autenticação
- `POST /api/v1/auth/login` - Fazer login

### Usuários
- `POST /api/v1/user/create` - Criar usuário
- `GET /api/v1/user/get/{email}` - Buscar usuário (autenticado)
- `PUT /api/v1/user/update/{email}` - Atualizar usuário (autenticado)
- `DELETE /api/v1/user/delete/{email}` - Deletar usuário (admin)

### Jogos
- `POST /api/v1/game/create` - Criar jogo (admin)
- `GET /api/v1/game/get/{name}` - Buscar jogo
- `PUT /api/v1/game/update/{name}` - Atualizar jogo (admin)
- `DELETE /api/v1/game/delete/{name}` - Deletar jogo (admin)

## ?? Autenticação

A API usa JWT Bearer Token. Para acessar endpoints protegidos:

1. Faça login em `/api/v1/auth/login`
2. Copie o token retornado
3. No Swagger, clique em "Authorize" e cole o token
4. Ou adicione o header: `Authorization: Bearer {seu-token}`

## ?? Roles

- **User**: Pode ver e editar seu próprio perfil
- **Admin**: Pode gerenciar jogos e usuários

## ??? Banco de Dados

### Criar o primeiro admin

Após rodar as migrations, execute no SQL Server:
```sql
INSERT INTO Clientes (Id, Name, Email, Password, Role, CreatedAt, LastUpdatedAt)
VALUES (
    NEWID(),
    'Admin',
    'admin@fcg.com',
    '$2a$11$examplehashhere', -- Use BCrypt para gerar o hash
    'Admin',
    GETUTCDATE(),
    GETUTCDATE()
);
```

Ou use o endpoint `/api/v1/user/create` e depois atualize a role manualmente:
```sql
UPDATE Clientes 
SET Role = 'Admin' 
WHERE Email = 'seu-email@email.com';
```

## ??? Desenvolvimento

### Adicionar nova migration
```bash
cd Fcg.Data
dotnet ef migrations add NomeDaMigration --startup-project ../Fcg.Api
dotnet ef database update --startup-project ../Fcg.Api
```

### Estrutura de pastas recomendada
```
Fcg.Domain/
??? Entities/          # Entidades do domínio
??? Interfaces/        # Contratos dos repositórios
??? GameHandlers/      # Handlers de jogos
??? UserHandlers/      # Handlers de usuários
```

## ????? Autor

**Manzoni**
- Email: pedromanzonidev@gmail.com
- GitHub: https://github.com/PedroManzoni
