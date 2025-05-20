

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


## 🔑 API Key Distinctions

This project uses two distinct types of API keys throughout its chat/completion proxy mechanism:

- **Client API Key (`api-key` header from the client):**
  - Supplied by the calling client application (e.g., via Swagger UI or API consumer).
  - This is created, stored, issued, and validated by this application (in your database, tied to users/clients, and checked by the validation handler/middleware).
  - There can be many of these, one per customer/user/application/etc.
  - This is the key end-users are instructed to use when invoking our proxy API.

- **Service API Key (used for Azure OpenAI internal calls):**
  - Held as a secret (in config/key vault), never exposed to clients or end-users.
  - Used by the backend to authenticate with Azure OpenAI when making the outbound API call.
  - There is typically only one of these, per environment/config.

> When referencing or designing API flows or documentation, always clarify if you mean client api-key (external-facing) or service api-key (internal, to Azure only).

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