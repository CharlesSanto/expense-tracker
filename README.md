# ExpenseTracker API

API REST para gerenciamento de despesas pessoais desenvolvida com **ASP.NET Core**.  
O projeto permite que usuários se registrem, façam login e gerenciem suas transações financeiras.

## Tecnologias utilizadas

- ASP.NET Core Web API
- C#
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- xUnit
- Moq
- FluentAssertions

## Arquitetura

O projeto segue uma arquitetura em camadas para separar responsabilidades:


- **Controllers**: recebem as requisições HTTP.
- **Services**: contêm a lógica de negócio.
- **Repositories**: fazem a comunicação com o banco de dados.
- **DTOs**: responsáveis pela transferência de dados entre as camadas.

## Autenticação

A API utiliza **JWT (JSON Web Token)** para autenticação.

Após realizar o login, o usuário recebe um token que deve ser enviado nas requisições protegidas.


## Funcionalidades

- Registro de usuários
- Login com autenticação JWT
- CRUD de usuários
- CRUD de transações
- Validação de acesso às transações por usuário

## Testes

O projeto possui testes unitários para a camada de serviços utilizando:

- **xUnit**
- **Moq**
- **FluentAssertions**

Serviços testados:

- AuthService
- UserService
- TransactionService
