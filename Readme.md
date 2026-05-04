# CleanArchitecture

A **.NET 10** template built with **Clean Architecture** principles and **Minimal APIs**. Designed as a ready-to-use starting point with auth, persistence, and a clean separation of concerns already wired up.

---

## 📐 Architecture Overview

Dependencies flow strictly inward — Domain knows nothing, Application knows only Domain, Infrastructure implements Application contracts, and the host wires everything together.

```
src/
├── CleanArchitecture/                   # API host — entry point & composition root
├── CleanArchitecture.Application/       # Use cases, contracts, DTOs, services
├── CleanArchitecture.Domain/            # Entities, enums, domain utilities
└── CleanArchitecture.Infrastructure/    # Persistence, security, DI wiring

tests/
└── CleanArchitecture.Application.Tests/ # Unit tests for application services
```

---

## 🗂️ Project Structure

### `CleanArchitecture` — API Host

```
CleanArchitecture/
├── Controllers/        # Minimal API endpoint groupings
├── Middleware/         # Custom middleware (error handling, logging, etc.)
├── appsettings.json
└── Program.cs          # App bootstrap, DI composition, middleware pipeline
```

### `CleanArchitecture.Application`

```
CleanArchitecture.Application/
├── Common/             # Shared abstractions, base classes, exceptions
├── Contracts/          # Interfaces for services and repositories
├── Dtos/               # Request & response data transfer objects
├── Services/           # Application service implementations (use case logic)
└── DependencyInjection.cs
```

### `CleanArchitecture.Domain`

```
CleanArchitecture.Domain/
├── Entities/           # Core domain entities
├── Enums/              # Domain-level enumerations
└── Utility/            # Domain helpers and shared value types
```

### `CleanArchitecture.Infrastructure`

```
CleanArchitecture.Infrastructure/
├── Persistence/        # DbContext, migrations, repository implementations
├── Security/           # Auth implementation (JWT, hashing, etc.)
└── DependencyInjection.cs
```

### `CleanArchitecture.Application.Tests`

```
CleanArchitecture.Application.Tests/
├── Fakes/              # In-memory fakes for repositories and services
├── AuthServiceTests.cs
├── BookServiceTests.cs
├── CartServiceTests.cs
└── GlobalUsings.cs
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- A relational database (configured via connection string)

### Clone & Run

```bash
git clone https://github.com/your-org/CleanArchitecture.git
cd CleanArchitecture

dotnet restore
dotnet build

cd src/CleanArchitecture
dotnet run
```

### Configuration

```bash
cp src/CleanArchitecture/appsettings.json src/CleanArchitecture/appsettings.Development.json
```

Update `appsettings.Development.json` with your connection string and any secrets (JWT key, etc.).

---

## 🧪 Running Tests

Tests are written against the Application layer using fakes — no database or web host required.

```bash
# Run all tests
dotnet test

# Run with output
dotnet test --logger "console;verbosity=detailed"

# Run only the application tests
dotnet test tests/CleanArchitecture.Application.Tests/
```

Test coverage includes:

- `AuthServiceTests` — registration, login, token flows
- `BookServiceTests` — book CRUD use cases
- `CartServiceTests` — cart operations and business rules

Fakes in the `Fakes/` folder replace real infrastructure (repositories, etc.) so tests stay fast and isolated.

---

## 🛠️ Tech Stack

| Concern | Technology |
|---|---|
| Runtime | .NET 10 |
| API style | Minimal APIs |
| Architecture | Clean Architecture |
| Database access | Entity Framework Core (via `Persistence/`) |
| Auth / Security | Custom security layer (`Security/`) |
| Testing | xUnit + in-memory fakes |

---

## ✅ Design Decisions

**Contracts over concretions** — The `Contracts/` folder in Application defines all interfaces. Infrastructure and tests depend on these, never on concrete implementations directly.

**Services, not handlers** — This template uses Application `Services/` for use case logic rather than a full CQRS mediator pattern, keeping the structure approachable.

**Fakes over mocks** — Tests use hand-written fakes from `Fakes/` instead of mocking libraries, making test intent clearer and reducing magic.

**Security in Infrastructure** — Auth concerns (JWT generation, password hashing) live in `Infrastructure/Security/`, keeping them out of Domain and Application.

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes following [Conventional Commits](https://www.conventionalcommits.org/)
4. Push and open a Pull Request

Keep Domain dependency-free. New contracts go in `Application/Contracts/`, implementations in `Infrastructure/`.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).