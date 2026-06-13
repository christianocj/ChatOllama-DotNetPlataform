# OllamaPlatform.NET

A modular, clean-architecture and web-ready chat client built with **.NET 10** to interact with local Large Language Models (LLMs) via **Ollama**. 

This project leverages the latest **`Microsoft.Extensions.AI`** ecosystem and the **`Microsoft Agent Framework`** to orchestrate conversational sessions with advanced features like dynamic model switching mid-conversation, system prompt behavior customization, and local persistence.

---

## Features

**Multi-Session Chat Management:** Create, rename, and manage independent conversation sessions. Pick up exactly where you left off.
* **Dynamic Model Switching:** Run infinite different models (e.g., Llama 3, Phi-3, Mistral) within a single session. Every individual assistant message tracks which model generated it.
* **Real-time Token Streaming:** Modern asynchronous token streaming using C#'s `IAsyncEnumerable` for an instant, responsive UI experience.
* **System Prompt Customization:** Define specific agent behaviors and roles (e.g., "Act as a software architecture professor", "Be technical", etc.) per session.
* **Persistent Local History:** Full conversation histories safely stored locally using Entity Framework Core (abstracted to allow seamless migration to Dapper or raw SQL).
* **Rich Markdown Rendering:** Native UI interpretation for titles, tables, code blocks, bullet points, and text emphasis.

---

## Architecture & Project Structure

The solution strictly follows **Clean Architecture** principles to ensure infrastructure components (like databases or AI orchestrators) can be swapped out without affecting the core application business rules.

## Architecture

┌─────────────────┐
│     Clients     │
├─────────────────┤
│ Blazor          │
│ WPF  (planned)  │
│ MAUI (planned)  │
└────────┬────────┘
         │
┌────────▼────────┐
│     Web API     │
└────────┬────────┘
         │
 ┌───────▼────────┐
 │  Application   │
 └───────┬────────┘
         │
 ┌───────▼────────┐
 │    Domain      │
 └───────┬────────┘
         │
 ┌───────▼────────┐
 │ Infrastructure │
 └───────┬────────┘
         │
 ┌───────▼────────┐
 │    Ollama      │
 └────────────────┘

 ## Technologies

- .NET 10
- ASP.NET Core Web API
- Blazor
- WPF
- Microsoft.Extensions.AI
- Microsoft Agent Framework
- Ollama
- Entity Framework Core
- SQLite

## Key Technical Specifications

### Database Performance

The persistence layer follows a hybrid identifier strategy designed for both performance and security:

- **Integer (`int`) primary keys** are used internally for database relationships and indexing.
- **GUID tokens** are exposed to API and client applications, preventing enumeration attacks and protecting internal database structure.
- Optimized for efficient joins, indexing, and scalability.

### Domain Isolation

The **Domain Layer** contains **zero third-party dependencies**, ensuring:

- Clean Architecture compliance.
- High testability.
- Business rules independent of infrastructure concerns.
- Easier migration between technologies.

### Fluent API Configuration

All database mappings are configured externally using **Entity Framework Core Fluent API**.

Benefits:

- Keeps entities clean and persistence-agnostic.
- Separates business logic from database concerns.
- Improves maintainability and flexibility.

---

## Tech Stack

| Category              | Technology                             |
|-----------            |----------------------------------------|
| Backend Framework     | .NET 10                                |
| AI Orchestration      | Microsoft.Extensions.AI                |
| Agent Framework       | Microsoft Agent Framework (MAF)        |
| Local LLM Runtime     | Ollama                                 |
| API Framework         | ASP.NET Core Web API                   |
| ORM / Persistence     | Entity Framework Core                  |
| Architecture          | Clean Architecture                     |
| Data Access Pattern   | Repository Pattern                     |
| Future Data Access    | Optimized for Dapper/Raw SQL migration |
| Frontend              | Blazor Server                          |
| Desktop Client        | WPF (Planned)                          |
| Cross-Platform Client | .NET MAUI (Planned)                    |

---

## Getting Started

### Prerequisites

Before running the project, ensure you have:

- .NET 10 SDK installed
- Ollama installed and running locally

Official Ollama server endpoint:

```text
http://localhost:11434
```

### Pull AI Models

Download one or more models using the terminal:

```bash
ollama pull llama3
ollama pull phi3
```

---

## Installation

### Clone the Repository

```bash
git clone https://github.com/christianocj/OllamaPlatform.NET.git
```

### Navigate to the API Project

```bash
cd ChatOllama/src/ChatOllama.Api
```

### Run the Application

```bash
dotnet run
```