# DIRS21 Mapping System

A .NET 8.0 solution for mapping data between DIRS21 internal models and partner formats (Google, Booking.com, etc.).

## Overview

This solution consists of two projects:
1. **DIRS21.Mapping** - Class library containing the mapping framework
2. **DIRS21.MappingConsoleUseCase** - Console application demonstrating the library usage

## Requirements

- .NET 8.0 SDK
- Visual Studio 2022

## DIRS21.Mapping - Class Library

The core mapping framework that can be used in any .NET application.

### Core Components

#### MapHandler
The main entry point for all mapping operations.

#### IMapper Interface
Contract for all mapper implementations.

#### MapperBase<TSource, TTarget>
Type-safe base class for creating mappers.

### Services

- **MapperRegistry**: Stores and retrieves registered mappers
- **MapperFactory**: Creates mapper instances
- **ValidationService**: Handles data validation
- **MapperInitializationService**: Auto-registers mappers and validators on startup

### Using the Library

#### 1. Install in Your Project
```xml
<ProjectReference Include="..\DIRS21.Mapping\DIRS21.Mapping.csproj" />
```

#### 2. Configure Services Using Extension Methods
The class library provides convenient extension methods for service registration:

```csharp
// In your Startup.cs or Program.cs
using DIRS21.Mapping.Extensions;

services.AddDIRS21Mapping();           // Registers core services
services.AddMappersFromAssembly();      // Auto-discovers and registers mappers
services.AddValidatorsFromAssembly();   // Auto-discovers and registers validators
```

## DIRS21.MappingConsoleUseCase - Console Application

A demonstration application showing how to use the mapping library.

### Running the Console App

```bash
cd DIRS21
donet build
dotnet run --project MappingConsoleUseCase
```

### What the Console App Demonstrates

1. **Basic Mapping**
   - DIRS21 Reservation → Google Reservation
   - Google Reservation → DIRS21 Reservation
   - DIRS21 Room → Google Room
   - Google Room → DIRS21 Room

2. **Validation**
   - Valid data scenarios
   - Invalid data handling
   - Custom validation rules

3. **Error Handling**
   - Missing mapper scenarios
   - Validation failures
   - Exception handling

4. **Auto-Registration**
   - mappers are automatically discovered
   - validators are automatically registered

### Console App Code Structure

```csharp
// Program.cs
using DIRS21.Mapping.Extensions;  // Extension methods from class library

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Using extension methods to register services
        services.AddDIRS21Mapping();           // Core services
        services.AddMappersFromAssembly();      // Auto-discover mappers
        services.AddValidatorsFromAssembly();   // Auto-discover validators
    })
    .Build();

await host.StartAsync();

// Get MapHandler and demonstrate usage
var mapHandler = host.Services.GetRequiredService<MapHandler>();

// Example mappings...
```

## Adding New Partners

New partners are added in the console application (or your own application) without modifying the class library.

### Example: Adding Booking.com Support

### 1. In Console App - Create Model
`MappingConsoleUseCase/Models/BookingReservation.cs`
```csharp
namespace DIRS21.MappingConsoleUseCase.Models
{
    public class BookingReservation
    {
        public string BookingReference { get; set; }
        public string GuestFullName { get; set; }
        public string ArrivalDate { get; set; }
    }
}
```

### 2. In Console App - Create Mapper
`MappingConsoleUseCase/MappersReservationToBookingMapper.cs`
```csharp
using DIRS21.Mapping.Core.Interfaces;
using DIRS21.Mapping.Models.Internal;
using DIRS21.MappingConsoleUseCase.Models;

namespace DIRS21.MappingConsoleUseCase.Mappers
{
    public class ReservationToBookingMapper : IMapper
    {
        public string SourceType => "Model.Reservation";
        public string TargetType => "Booking.Reservation";

        public object Map(object source)
        {
            var reservation = source as Reservation;
            return new BookingReservation
            {
                BookingRef = reservation.Id,
                GuestFullName = reservation.GuestName,
                ArrivalDate = reservation.CheckIn.ToString("yyyy-MM-dd"),
                DepartureDate = reservation.CheckOut.ToString("yyyy-MM-dd")
            };
        }
    }
}

```

### 3. In Console App - Create Validator (Optional)
`DIRS21.MappingConsoleUseCase/Validation/BookingReservationValidator.cs`
```csharp
using DIRS21.Mapping.Validation;
using DIRS21.MappingConsoleUseCase.Models;

namespace DIRS21.MappingConsoleUseCase.Validators
{
    public class BookingReservationValidator : IValidatorType
    {
        public string TypeName => "Booking.Reservation";

        public ValidationResult Validate(object data)
        {
            ---Validation Code---
            return errors.Any()
                ? ValidationResult.Failure(errors.ToArray())
                : ValidationResult.Success();
        }
    }
}

```

### 4. Use It
The mapper and validator are automatically discovered when the console app starts!
```csharp
// In Program.cs
var mapped = mapHandler.Map(reservation, "Model.Reservation", "Booking.Reservation");
```

## How Auto-Registration Works

1. On application startup, `ServiceCollectionExtensions` and `MapperInitializationService` runs
2. It scans **all loaded assemblies** including:
   - DIRS21.Mapping.dll (contains Google mappers)
   - DIRS21.Mapping.ConsoleApp.exe (contains Booking mappers)
3. Finds all classes implementing `IMapper` and `ITypedValidator`
4. Registers them automatically
5. No manual registration code needed!

## Project Responsibilities

### DIRS21.Mapping (Class Library)
- **Core Framework**: MapHandler, interfaces, base classes
- **Services**: Registry, Factory, Validation
- **Google Implementation**: As an example/default partner
- **Reusable**: Can be used in any .NET project

### DIRS21.MappingConsoleUseCase (Console Application)
- **Demonstrates**: How to use the mapping library
- **New Partners**: Add Booking, Expedia, etc. here
- **Testing**: Shows validation and error handling
- **Example**: How other applications would use the library

## Currently Implemented Mappings

| Source | Target | Location |
|--------|--------|----------|
| Model.Reservation | Google.Reservation | Class Library |
| Google.Reservation | Model.Reservation | Class Library |
| Model.Room | Google.Room | Class Library |
| Google.Room | Model.Room | Class Library |
| Model.Reservation | Booking.Reservation | Console App (Extension Example) |


## Key Features

- **Class Library**: Reusable in any .NET project
- **Console App**: Working demonstration
- **Auto-discovery**: No manual registration via `ServiceCollectionExtensions`
- **Clean DI Integration**: Simple service registration with extension methods
- **Zero modification**: Extend without changing existing code
- **Type safety**: Compile-time checking
- **Validation**: Built-in data validation
- **SOLID principles**: Clean architecture

## Getting Started

1. **Build the Solution**
   ```bash
   dotnet build
   ```

2. **Run Console Demo**
   ```bash
   dotnet run --project MappingConsoleUseCase
   ```

3. **Use in Your Project**
   - Reference the DIRS21.Mapping class library
   - Configure services
   - Inject MapHandler where needed
