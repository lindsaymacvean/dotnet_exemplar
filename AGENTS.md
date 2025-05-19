

# AGENTS.md

## 🎯 Project Purpose

This boilerplate project demonstrates a clean architecture .NET application built for CV and code review purposes. It is designed to showcase technical competency in modern C#/.NET development, including layering, dependency injection, testing, CQRS, and integration with external services like AI APIs.

The user (acting as a technical architect) will also use this project to validate code review capabilities internally at a corporation with restricted infrastructure. A secondary track will involve building a code review assistant using Azure Cognitive Services, potentially integrating with this project in the future.

---

## 🤖 Agents

### 🧠 CodeReviewerAgent

- **Goal**: Analyze C# and .NET source code for adherence to clean architecture principles.
- **Scope**:
  - Ensure proper layering: Domain, Application, Infrastructure, API
  - Flag tight coupling or violations of separation of concerns
  - Confirm correct use of dependency injection and CQRS
- **Behavior**: Strict and detailed, prioritizing long-term maintainability

---

### 🧪 TestAgent

- **Goal**: Generate or validate unit and integration tests.
- **Focus**:
  - xUnit and FluentAssertions usage
  - Ensure every use case has corresponding tests
  - Maintain fast feedback loops
- **Style**: Clear, focused tests, minimal mocking where appropriate

---

### 🔌 IntegrationAgent

- **Goal**: Assist in integrating external systems such as AI services or Redis.
- **Focus**:
  - Interface with Azure OpenAI or other services
  - Setup and validate Redis-backed session handling
  - Handle environmental config with minimal manual steps

---

## 🧱 Development Plan Overview

1. Scaffold project with clean architecture layers e.g. jason taylor template
2. Connect to MSSQL using EF Core and LINQ; implement migrations
3. Add DI setup and service registration
4. Implement Redis-backed session and authentication
5. Write xUnit-based tests for application layer logic
6. Implement CQRS via MediatR
7. Interface with a third-party service (e.g., AI analysis API)