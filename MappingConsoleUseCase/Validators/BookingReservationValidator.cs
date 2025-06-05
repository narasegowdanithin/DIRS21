using DIRS21.Mapping.Validation;
using DIRS21.MappingConsoleUseCase.Models;

namespace DIRS21.MappingConsoleUseCase.Validators
{
    public class BookingReservationValidator : IValidatorType
    {
        public string TypeName => "Booking.Reservation";

        public ValidationResult Validate(object data)
        {
            if (!(data is BookingReservation bookingReservation))
            {
                return ValidationResult.Failure("Invalid data type - expected GoogleReservation");
            }

            var errors = new List<string>();

            // Validate ReservationId
            if (string.IsNullOrWhiteSpace(bookingReservation.BookingRef))
            {
                errors.Add("Reservation ID is required");
            }
            // Validate Guest
            if (string.IsNullOrWhiteSpace(bookingReservation.GuestFullName))
            {
                errors.Add("Guest name is required");
            }


            // Validate StartDate
            if (string.IsNullOrWhiteSpace(bookingReservation.ArrivalDate))
            {
                errors.Add("Start date is required");
            }
            else
            {
                if (!DateTime.TryParse(bookingReservation.ArrivalDate, out var startDate))
                {
                    errors.Add("Start date must be a valid date in format yyyy-MM-dd");
                }
                else if (startDate < DateTime.Now.Date)
                {
                    errors.Add("Start date cannot be in the past");
                }
            }

            // Validate EndDate
            if (string.IsNullOrWhiteSpace(bookingReservation.DepartureDate))
            {
                errors.Add("End date is required");
            }
            else
            {
                if (!DateTime.TryParse(bookingReservation.DepartureDate, out var endDate))
                {
                    errors.Add("End date must be a valid date in format yyyy-MM-dd");
                }
                else if (!string.IsNullOrWhiteSpace(bookingReservation.ArrivalDate) &&
                         DateTime.TryParse(bookingReservation.ArrivalDate, out var startDate))
                {
                    if (endDate <= startDate)
                    {
                        errors.Add("End date must be after start date");
                    }
                }
            }

            return errors.Any()
                ? ValidationResult.Failure(errors.ToArray())
                : ValidationResult.Success();
        }
    }
}
