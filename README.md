# .NET Clean Architecture Exemplar

## 🎯 Project Purpose

This boilerplate project demonstrates a clean architecture .NET application built for CV and code review purposes. It is designed to showcase technical competency in modern C#/.NET development, including layering, dependency injection, testing, CQRS, and integration with external services like AI APIs.

The user (acting as a technical architect) will also use this project to validate code review capabilities internally in a corporate environment with restricted infrastructure. A secondary track will involve building a code review assistant using Azure Cognitive Services, potentially integrating with this project in the future.

## 🧱 Development Plan Overview

1. Scaffold project with clean architecture layers e.g. jason taylor template
2. Connect to MSSQL using EF Core and LINQ; implement migrations
3. Add DI setup and service registration
4. Implement Redis-backed session and authentication
5. Write xUnit-based tests for application layer logic
6. Implement CQRS via MediatR
7. Interface with a third-party service (e.g., AI analysis API)

## 🛠️ Development Setup

### VS Code Extensions
```bash
code --install-extension ms-dotnettools.csharp
code --install-extension ms-dotnettools.csdevkit
```

## 🐳 Database Setup with Docker

This project uses SQL Server running in Docker. Follow these steps to set up the database:

1. Install Docker Desktop if you haven't already: https://www.docker.com/products/docker-desktop

2. Start SQL Server container:
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong\!Passw0rd" -p 1433:1433 --name sql_server --hostname sql_server -d mcr.microsoft.com/mssql/server:2022-latest
```

3. Verify the container is running:
```bash
docker ps
```

4. The connection string in `appsettings.Development.json` is already configured to use this SQL Server instance. If you want to query manually then try [DBeaver](https://dbeaver.io/download/)

5. To stop the container when you're done:
```bash
docker stop sql_server
```

## 🔐 Local Development – Azure API Key Setup
This project uses **.NET User Secrets** to store the Azure OpenAI API key securely during development.
1. Initialize User Secrets (only once per repo) 
`dotnet user-secrets init --project src/Web` 
2. Set your Azure API key 
`dotnet user-secrets set "AzureOpenAI:ApiKey" "your-azure-api-key-here" --project src/Web` 
`dotnet user-secrets set "AzureOpenAI:Endpoint" "https://your-instance.openai.azure.com/openai" --project src/Web`
The key is stored securely on your machine (outside the codebase) and is never committed to Git 

## Build
Run `dotnet build -tl` to build the solution.

## Run
To run the web application:
```bash
dotnet watch --project src/Web run
```
Login with administrator@localhost:Administrator1!
(Obviously reset this after initial deploy to production)

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Code Styles & Formatting
The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

Create a new command:
```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:
```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

Create a new migration (from an infrastructure entity and DbContext)
```
dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/Web
dotnet ef database update --project src/Infrastructure --startup-project src/Web
```

## Test

The solution contains unit, integration, functional, and acceptance tests.

To run the unit, integration, and functional tests (excluding acceptance tests):
```bash
dotnet test --filter "FullyQualifiedName!~AcceptanceTests"
```

To run the acceptance tests, first start the application:

```bash
cd .\src\Web\
dotnet run
```

Then, in a new console, run the tests:
```bash
cd .\src\Web\
dotnet test
```

## 🚀 First time Production Deployment Checklist

Before deploying to production, make sure to:

1. **Set the ASP.NET Core environment variable to Production**  
   This ensures that production-specific settings and behaviors are used.

   ```bash
   export ASPNETCORE_ENVIRONMENT=Production

2. Set up mssql database and azure secrets
3. Reset admin password