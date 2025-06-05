# DIRS21 Mapping System

A .NET 8.0 solution for mapping data between DIRS21 internal models and partner formats (Google, Booking.com, etc.).

## Overview

This solution consists of two projects:
1. **DIRS21.Mapping** - Class library containing the mapping framework
2. **DIRS21.Mapping.ConsoleApp** - Console application demonstrating the library usage

## Requirements

- .NET 8.0
- Visual Studio 2022 or VS Code

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

#### 2. Configure Services
```csharp
// In your Startup.cs or Program.cs
services.AddDIRS21Mapping();
services.AddMappersFromAssembly();
services.AddValidatorsFromAssembly();
```

## DIRS21.Mapping.ConsoleApp - Console Application

A demonstration application showing how to use the mapping library.

### Running the Console App

```bash
cd DIRS21.MappingSystem
dotnet run --project src/DIRS21.MappingConsoleApp
```

### What the Console App Demonstrates

1. **Basic Mapping**
   - DIRS21 Reservation → Google Reservation 
   - Google Reservation → DIRS21 Reservation
   - Room mappings

2. **Validation**
   - Valid data scenarios
   - Invalid data handling
   - Custom validation rules

3. **Error Handling**
   - Missing mapper scenarios
   - Validation failures
   - Exception handling

4. **Auto-Registration**
   - How mappers are automatically discovered
   - How validators are automatically registered

### Console App Code Structure

```csharp
// Program.cs
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Add mapping services
        services.AddDIRS21Mapping();
        services.AddMappersFromAssembly();
        services.AddValidatorsFromAssembly();
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
`DIRS21.Mapping.ConsoleApp/Models/BookingReservation.cs`
```csharp
namespace DIRS21.Mapping.ConsoleApp.Models
{
    public class BookingReservation
    {
        public string BookingRef { get; set; }
        public string GuestFullName { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }
    }
}
```

### 2. In Console App - Create Mapper
`DIRS21.Mapping.ConsoleApp/Mappers/ReservationToBookingMapper.cs`
```csharp
using DIRS21.Mapping.Core.Base;
using DIRS21.Mapping.Models.Internal;
using DIRS21.Mapping.ConsoleApp.Models.External.Booking;

public class ReservationToBookingMapper : MapperBase<Reservation, BookingReservation>
{
    public override string SourceType => "Model.Reservation";
    public override string TargetType => "Booking.Reservation";

    protected override BookingReservation MapInternal(Reservation source)
    {
        return new BookingReservation
        {
          BookingRef = reservation.Id,
          GuestFullName = reservation.GuestName,
          ArrivalDate = reservation.CheckIn.ToString("yyyy-MM-dd"),
          DepartureDate = reservation.CheckOut.ToString("yyyy-MM-dd")
        };
    }
}
```

### 3. In Console App - Create Validator (Optional)
`DIRS21.Mapping.ConsoleApp/Validation/BookingReservationValidator.cs`
```csharp
using DIRS21.Mapping.Validation;
using DIRS21.Mapping.ConsoleApp.Models.External.Booking;

public class BookingReservationValidator : ITypedValidator
{
    public string TypeName => "Booking.Reservation";
    
    public ValidationResult Validate(object data)
    {
        
        ----
          More code on validation
        ----
            
        return errors.Any() 
            ? ValidationResult.Failure(errors.ToArray())
            : ValidationResult.Success();
    }
}
```

### 4. Use It
The mapper and validator are automatically discovered when the console app starts!
```csharp
// In Program.cs
var bookingData = mapHandler.Map(reservation, "Model.Reservation", "Booking.Reservation");
```

## How Auto-Registration Works

1. On application startup, `MapperInitializationService` runs
2. It scans **all loaded assemblies** including:
   - DIRS21.Mapping.dll (contains Google mappers)
   - DIRS21.Mapping.ConsoleApp.exe (contains Booking mappers)
3. Finds all classes implementing `IMapper` and `ITypedValidator`
4. Registers them automatically
5. No manual registration code needed!

## Project Responsibilities

### DIRS21.Mapping (Class Library)
- **Core Framework**: MapHandler, interfaces, base class, exceptions
- **Services**: Registry, Factory, Validation
- **Google Implementation**: As an example/default partner
- **Reusable**: Can be used in any .NET project

### DIRS21.Mapping.ConsoleApp (Console Application)
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
| Model.Reservation | Booking.Reservation | Console App (Example) |
| Booking.Reservation | Model.Reservation | Console App (Example) |

## Key Features

- **Class Library**: Reusable in any .NET project
- **Console App**: Working demonstration
- **Auto-discovery**: No manual registration
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
   dotnet run --project src/DIRS21.Mapping.ConsoleApp
   ```

3. **Use in Your Project**
   - Reference the DIRS21.Mapping class library
   - Configure services
   - Inject MapHandler where needed
