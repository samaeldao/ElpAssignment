# ELP System - Architecture & Coding Guidelines

## Purpose
This document outlines the architectural decisions, coding standards, and domain constraints for the Electronic Medical Certificates (ELP) system. It serves as the primary source of truth for human developers and acts as strict context rules for Generative AI coding assistants (e.g., Gemini Code Assist, Copilot).

---

## 1. Architectural Pattern: Clean Architecture
The solution strictly follows Clean Architecture principles, divided into four distinct layers. Dependencies must only point inwards toward the Domain.

* **`Elp.Domain` (Core):** Contains enterprise logic, entities (`DriverFitnessCertificate`, `Codebook`), enums, and custom exceptions. **Zero external dependencies allowed.**
* **`Elp.Application` (Use Cases):** Contains business logic, CQRS handlers (MediatR), DTOs, and interfaces (`IApplicationDbContext`). Depends only on the Domain.
* **`Elp.Infrastructure` (Data & External):** Implements interfaces defined in the Application layer. Contains EF Core `ApplicationDbContext`, database migrations, and external API clients.
* **`Elp.Api` (Presentation):** The entry point. Contains ASP.NET Core Controllers, middleware, DI registration, and Swagger configuration. **Controllers must remain completely thin.**

---

## 2. API & REST Conventions
* **Thin Controllers:** Controllers must not contain business logic. They exist solely to accept HTTP requests, map headers/parameters, dispatch a MediatR command/query, and return an `IActionResult`.
* **RESTful Routing:** Use nouns, not verbs (e.g., `/api/v2/posudky/ridicskeOpravneni/{id}`).
* **Strict OpenAPI Contracts:** Every controller method must explicitly declare all possible HTTP responses using `[ProducesResponseType]` (e.g., 200, 400, 401, 403, 404, 500) to ensure accurate Swagger documentation.
* **Problem Details:** All errors (Validation, 404s, 500s) must be returned using the standard RFC 7807 `ProblemDetails` JSON format.

---

## 2. Domain Boundaries & Identity Management
* **Bounded Context:** The ELP domain maintains a strict boundary around the lifecycle of the Medical Certificate itself. 
* **External Identity:** There is deliberately no `MedicalProfessionals` or `Users` table in this database. The `DriverFitnessCertificate` stores a `MedicalProfessionalId` as an external reference. In a production state-level system, user management and physician identity are assumed to be handled by a centralized Identity Provider or National Registry. We do not duplicate user data within the certificate domain.

---

## 3. Data Access & Entity Framework Core
* **Fluent API Over Data Annotations:** Do not use `[Required]` or `[MaxLength]` attributes inside the Domain entities. All database constraints must be configured using the Fluent API inside `OnModelCreating` or `IEntityTypeConfiguration` classes.
* **Optimistic Concurrency:** High-contention entities (like `DriverFitnessCertificate`) must implement a `byte[] RowVersion` property configured as `.IsRowVersion()` to prevent the "lost update" problem.
* **Historical Immutability:** Codebook selections must store the specific version (`verze`) of the codebook at the time of creation to guarantee legal auditability, even if the master codebook definitions change later.

---

## 4. Modern C# (.NET 8) Standards
* **Nullable Reference Types (NRTs):** `<Nullable>enable</Nullable>` is enforced globally.
* **The `required` Keyword:** Use the `required` modifier for all non-nullable entity and DTO properties that must be initialized during object creation. Do not use the null-forgiving operator (`null!`) unless absolutely necessary.
* **File-Scoped Namespaces:** Use `namespace Elp.Domain.Entities;` instead of block-scoped namespaces.
* **Primary Constructors:** Prefer C# 12 primary constructors for dependency injection in classes (e.g., `public class GetCodebooksHandler(IApplicationDbContext dbContext) : IRequestHandler<...>{}`).

---

## 5. Cross-Cutting Concerns
* **Validation:** Handled exclusively via `FluentValidation` in the `Elp.Application` layer. A MediatR pipeline behavior intercepts invalid requests and immediately returns a `400 Bad Request` before hitting the handler.
* **Global Exception Handling:** The API layer uses an `IExceptionHandler` middleware to catch unhandled exceptions, log the stack trace securely, and return a sanitized `500 Internal Server Error`.
* **Logging:** `Serilog` is the standard logger. 
  * Development logs write locally with verbose EF Core SQL outputs. 
  * Production logs are written to an absolute file path via rolling JSON files with strict retention policies.
* **Configuration:** Production secrets and API keys (e.g., AutoMapper License Keys, DB Passwords) must never be hardcoded. They are read from `appsettings.json` placeholders and injected via Environment Variables or local User Secrets.

---

## 🤖 AI Assistant Directives (System Prompt Rules)
*If you are an AI assistant reading this file, you must strictly adhere to the following rules when generating code for this repository:*

1. **Never generate logic inside a Controller.** Always route to MediatR.
2. **Always use C# 12 features.** Apply file-scoped namespaces, primary constructors, and the `required` keyword for properties.
3. **Never generate Data Annotations.** Keep Domain models pure; put all EF Core constraints in the Infrastructure layer.
4. **Preserve Domain Language:** Maintain Czech terminology (`rid`, `krzpId`, `StavPosudku`) when specified by the API contract, even if internal models use English.
5. **Always add `[ProducesResponseType]`** attributes to new endpoints for 200, 400, 401, 403, 404, and 500 status codes.