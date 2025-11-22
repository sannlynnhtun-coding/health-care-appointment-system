# Healthcare Appointment System (HCAS)

A comprehensive healthcare appointment management system built with .NET 8, implementing the Mediator Pattern using MediatR for clean separation of concerns and CQRS architecture.

## Tech Stack

### Backend
- **.NET 8.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API layer
- **Entity Framework Core 9.0.9** - ORM for database operations
- **SQL Server** - Database
- **MediatR 13.1.0** - Mediator pattern implementation
- **Dapper 2.1.66** - Micro ORM for high-performance queries
- **Swagger/OpenAPI** - API documentation

### Frontend
- **Blazor** - Web framework for building interactive web UIs
- **MudBlazor 8.12.0** - Material Design component library
- **Radzen.Blazor 7.3.5** - Enterprise UI components
- **Highcharts** - Charting library for dashboards

### Architecture Patterns
- **Mediator Pattern** (via MediatR) - Decouples controllers from business logic
- **CQRS** (Command Query Responsibility Segregation) - Separates read and write operations
- **Result Pattern** - Consistent error handling and response structure
- **Feature-based Organization** - Grouped by domain features

## Project Structure

```
health-care-appointment-system/
├── HCAS.Api/              # Web API controllers and endpoints
├── HCAS.Domain/           # Business logic, commands, queries, handlers
├── HCAS.Database/         # Entity Framework DbContext and database models
├── HCAS.Shared/           # Shared utilities (Result pattern, DapperService)
└── HCAS.WebApp/           # Blazor web application (UI)
```

## Mediator Pattern Implementation

This project uses **MediatR** to implement the Mediator Pattern, which provides the following benefits:

### Why Mediator Pattern?
- **Decoupling**: Controllers don't directly depend on business logic services
- **Single Responsibility**: Each handler handles one specific command or query
- **Testability**: Easy to unit test handlers independently
- **Maintainability**: Clear separation between API layer and domain logic
- **CQRS Support**: Natural separation of commands (write) and queries (read)

### How It Works

1. **Commands & Queries**: Request objects that carry data
   - `IRequest<TResult>` - Marker interface for requests
   - Commands: Modify state (Create, Update, Delete)
   - Queries: Read data without side effects

2. **Handlers**: Process commands/queries
   - `IRequestHandler<TRequest, TResult>` - Handles the request
   - One handler per command/query
   - Contains business logic or delegates to services

3. **Mediator**: Routes requests to appropriate handlers
   - Automatically discovered and registered via assembly scanning
   - Controllers send requests through `IMediator.Send()`

### Example Flow

```csharp
// 1. Command/Query Definition
public class RegisterDoctorCommand : IRequest<Result<DoctorsResModel>>
{
    public string Name { get; set; }
    public int SpecializationId { get; set; }
}

// 2. Handler Implementation
public class RegisterDoctorCommandHandler : IRequestHandler<RegisterDoctorCommand, Result<DoctorsResModel>>
{
    private readonly AppDbContext _dbContext;
    
    public async Task<Result<DoctorsResModel>> Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
    {
        // Business logic here
        var service = new DoctorService(_dbContext);
        return await service.RegisterDoctorAsync(dto);
    }
}

// 3. Controller Usage
[HttpPost]
public async Task<IActionResult> RegisterDoctor([FromBody] DoctorsReqModel dto)
{
    var command = new RegisterDoctorCommand { Name = dto.Name, ... };
    var result = await _mediator.Send(command);
    return Execute(result);
}
```

## Project Flow

### Request Flow (CQRS + Mediator)

```
┌─────────────┐
│   Client    │  HTTP Request
└──────┬──────┘
       │
       ▼
┌─────────────┐
│  Controller │  Creates Command/Query
└──────┬──────┘
       │
       ▼
┌─────────────┐
│   Mediator  │  Routes to Handler
└──────┬──────┘
       │
       ▼
┌─────────────┐
│   Handler   │  Executes Business Logic
└──────┬──────┘
       │
       ▼
┌─────────────┐
│   Service   │  Domain Logic & Validation
└──────┬──────┘
       │
       ▼
┌─────────────┐
│  DbContext  │  Data Access (EF Core)
│  or Dapper  │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│   Result<T> │  Response Wrapper
└──────┬──────┘
       │
       ▼
┌─────────────┐
│   Client    │  HTTP Response
└─────────────┘
```

### Detailed Flow Steps

1. **API Layer** (`HCAS.Api`)
   - Receives HTTP request
   - Validates input model
   - Creates Command/Query object
   - Sends via `IMediator.Send()`

2. **Domain Layer** (`HCAS.Domain`)
   - **Feature Structure**: Each domain feature (Doctors, Patients, Appointments, etc.) contains:
     - `Commands/` - Write operations (Create, Update, Delete)
     - `Queries/` - Read operations (GetAll, GetById, etc.)
     - `Handlers/` - Command/Query handlers
     - `Models/` - Request/Response DTOs
     - `Services/` - Business logic services

3. **Handler Execution**
   - MediatR automatically routes to correct handler
   - Handler may use Service classes for complex logic
   - Handler accesses DbContext for data operations

4. **Data Access**
   - **Entity Framework Core**: Used for CRUD operations
   - **Dapper**: Used for complex queries and performance-critical operations
   - Both use same `AppDbContext` connection

5. **Response Handling**
   - Uses `Result<T>` pattern for consistent responses
   - Contains: `IsSuccess`, `Data`, `Message`, `Type` (Success/ValidationError/SystemError/NotFound)
   - BaseController handles HTTP status code mapping

## Features

The system manages:
- **Doctors** - Doctor registration and management
- **Patients** - Patient registration and management  
- **Appointments** - Appointment booking and status tracking
- **Doctor Schedules** - Schedule management for doctors
- **Specializations** - Medical specializations
- **Staff** - Administrative staff management

## Database Setup

### Scaffold Database Context
```bash
dotnet tool install --global dotnet-ef
```

```bash
dotnet ef dbcontext scaffold "Server=.;Database=HealthCareDB;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContextModels -c AppDbContext -f
```

## Git Commands

### Clear Git Cache
```bash
git rm -r --cached .
git add .
git commit -am 'fix: git cache cleared'
git push
```