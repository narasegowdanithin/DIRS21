using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DIRS21.Mapping.Core;
using DIRS21.Mapping.Core.Exceptions;
using DIRS21.Mapping.Extensions;
using DIRS21.Mapping.Models.External.Google;
using DIRS21.Mapping.Models.Internal;
using MappingConsoleUseCase.Models;
using DIRS21.Mapping.Core.Interfaces;
using DIRS21.Mapping.Validation;

namespace DIRS21.Mapping.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Register specific mappers manually
                    //services.AddSingleton<IMapper, ReservationToBookingMapper>();
                    // Register specific validators manually
                    //services.AddSingleton<IValidatorType, BookingReservationValidator>();
                    // Add core mapping services

                    services.AddDIRS21Mapping();

                    // Register mappers from all assemblies
                    services.AddMappersFromAssembly();

                    // Register validators from all assemblies
                    services.AddValidatorsFromAssembly();

                    // Configure logging
                    services.AddLogging(builder =>
                    {
                        builder.SetMinimumLevel(LogLevel.Information);
                        builder.AddConsole();
                    });
                })
                .Build();

            // Start the host which triggers MapperInitializationService
            await host.StartAsync();
            var registry = host.Services.GetRequiredService<IMapperRegistry>();
            var mappings = registry.GetRegisteredMappings();
            Console.WriteLine($"Total registered mappings: {mappings.Count()}");
            foreach (var mapping in mappings)
            {
                Console.WriteLine($"  - {mapping}");
            }
            //var validators= host.Services.GetRequiredService<IValidationService>().ToLi;
            var validators = host.Services.GetServices<IValidatorType>().ToList();
            Console.WriteLine($"Total registered validators: {validators.Count()}");
            foreach (var validator in validators)
            {
                Console.WriteLine($"  - {validator.TypeName}");
            }

            // Get the MapHandler
            var mapHandler = host.Services.GetRequiredService<MapHandler>();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            // Test Case 0: Valid Booking  Reservation
            Console.WriteLine("=== Test Case 0: Valid Booking Reservation ===");
            try
            {
                var reservation1 = new Reservation
                {
                    Id = "RES-789",
                    GuestName = "Diana Prince",
                    CheckIn = DateTime.Now.AddDays(10),
                    CheckOut = DateTime.Now.AddDays(15)
                };
                var mapped = mapHandler.Map(reservation1, "Model.Reservation", "Booking.Reservation");

                if (mapped is BookingReservation internalRes)
                {
                    Console.WriteLine($" Mapping Successful!");
                    Console.WriteLine($"  Mapped Id: {internalRes.BookingRef}");
                    Console.WriteLine($"  Guest: {internalRes.GuestFullName}");
                    Console.WriteLine($"  Check In: {internalRes.ArrivalDate}");
                    Console.WriteLine($"  Check Out: {internalRes.DepartureDate}");
                }
            }
            catch (MappingValidationException ex)
            {
                Console.WriteLine($" Validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error: {ex.Message}");
            }


            // Test Case 1: Valid Google Reservation
           Console.WriteLine("=== Test Case 1: Valid Google Reservation ===");
            try
            {
                var validGoogleReservation = new GoogleReservation
                {
                    ReservationId = "G-123",
                    Guest = "Alice Johnson",
                    StartDate = "2025-06-01",
                    EndDate = "2025-06-05"
                };

                var mapped = mapHandler.Map(validGoogleReservation, "Google.Reservation", "Model.Reservation");

                if (mapped is Reservation internalRes)
                {
                    Console.WriteLine($" Mapping Successful!");
                    Console.WriteLine($"  Mapped Id: {internalRes.Id}");
                    Console.WriteLine($"  Guest: {internalRes.GuestName}");
                    Console.WriteLine($"  Check In: {internalRes.CheckIn:yyyy-MM-dd}");
                    Console.WriteLine($"  Check Out: {internalRes.CheckOut:yyyy-MM-dd}");
                }
            }
            catch (MappingValidationException ex)
            {
                Console.WriteLine($"✗ Validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error: {ex.Message}");
            }

            // Test Case 2: Invalid Google Reservation (null ID)
            Console.WriteLine("\n=== Test Case 2: Invalid Google Reservation (null ID) ===");
            try
            {
                var invalidGoogleReservation = new GoogleReservation
                {
                    ReservationId = null, // Invalid
                    Guest = "Bob Smith",
                    StartDate = "2025-06-01",
                    EndDate = "2025-06-05"
                };

                var mapped = mapHandler.Map(invalidGoogleReservation, "Google.Reservation", "Model.Reservation");
                Console.WriteLine(" Mapping Successful (unexpected - validation should have failed)");
            }
            catch (MappingValidationException ex)
            {
                Console.WriteLine($" Validation failed (expected): {ex.Message}");
            }

            // Test Case 3: Invalid dates
            Console.WriteLine("\n=== Test Case 3: Invalid Google Reservation (bad dates) ===");
            try
            {
                var invalidDatesReservation = new GoogleReservation
                {
                    ReservationId = "GOOG-456",
                    Guest = "Charlie Brown",
                    StartDate = "2025-06-01",
                    EndDate = "not-a-date" // Invalid date format
                };

                var mapped = mapHandler.Map(invalidDatesReservation, "Google.Reservation", "Model.Reservation");
                Console.WriteLine(" Mapping Successful (unexpected)");
            }
            catch (MappingValidationException ex)
            {
                Console.WriteLine($" Validation failed (expected): {ex.Message}");
            }

            // Test Case 4: Test DIRS21 to Google mapping with validation
            Console.WriteLine("\n=== Test Case 4: DIRS21 to Google Mapping ===");
            try
            {
                var reservation = new Reservation
                {
                    Id = "RES-789",
                    GuestName = "Diana Prince",
                    CheckIn = DateTime.Now.AddDays(10),
                    CheckOut = DateTime.Now.AddDays(15)
                };

                var googleMapped = mapHandler.Map(reservation, "Model.Reservation", "Google.Reservation");

                if (googleMapped is GoogleReservation googleRes)
                {
                    Console.WriteLine($" Mapping Successful!");
                    Console.WriteLine($"  Google ID: {googleRes.ReservationId}");
                    Console.WriteLine($"  Guest: {googleRes.Guest}");
                    Console.WriteLine($"  Start Date: {googleRes.StartDate}");
                    Console.WriteLine($"  End Date: {googleRes.EndDate}");
                }
            }
            catch (MappingValidationException ex)
            {
                Console.WriteLine($" Validation failed: {ex.Message}");
            }

            // Test Case 5: Invalid DIRS21 Reservation
            Console.WriteLine("\n=== Test Case 5: Invalid DIRS21 Reservation ===");
            try
            {
                var invalidReservation = new Reservation
                {
                    Id = "", // Empty ID
                    GuestName = null, // Null guest name
                    CheckIn = DateTime.Now.AddDays(-5), // Past date
                    CheckOut = DateTime.Now.AddDays(-3)
                };

                var mapped = mapHandler.Map(invalidReservation, "Model.Reservation", "Google.Reservation");
                Console.WriteLine(" Mapping Successful (unexpected)");
            }
            catch (MappingValidationException ex)
            {
                Console.WriteLine($" Validation failed (expected): {ex.Message}");
            }

            Console.WriteLine("=== ROOM VALIDATOR TESTING ===\n");

            // Test Case 1: Valid Room
            Console.WriteLine("Test 1: Valid Room");
            try
            {
                var validRoom = new Room
                {
                    RoomId = "ROOM-101",
                    Type = "Deluxe Suite",
                    Price = 299.99m
                };

                var result = mapHandler.Map(validRoom, "Model.Room", "Google.Room");
                Console.WriteLine("SUCCESS - Room validation passed and mapped successfully\n");
            }
            catch (MappingValidationException ex)
            {
                Console.WriteLine($" FAILED - {ex.Message}\n");
            }

            // Test Case 2: Invalid Room (negative price)
            Console.WriteLine("Test 2: Invalid Room (negative price)");
            try
            {
                var invalidRoom = new Room
                {
                    RoomId = "ROOM-102",
                    Type = "Standard Room",
                    Price = -50m  // Invalid: negative price
                };

                var result = mapHandler.Map(invalidRoom, "Model.Room", "Google.Room");
                Console.WriteLine(" Unexpected - Mapping should have failed\n");
            }
            catch (MappingValidationException ex)
            {
                Console.WriteLine($" EXPECTED FAILURE - {ex.Message}\n");
            }


            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();

            // Gracefully stop the host
            await host.StopAsync();
        }
    }
}