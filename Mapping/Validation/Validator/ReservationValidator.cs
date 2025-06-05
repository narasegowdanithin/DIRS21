using DIRS21.Mapping.Models.Internal;

namespace DIRS21.Mapping.Validation.Validators
{
    /// <summary>
    /// Validator for DIRS21 Reservation model
    /// </summary>
    public class ReservationValidator : IValidatorType
    {
        public string TypeName => "Model.Reservation";

        public ValidationResult Validate(object data)
        {
            if (!(data is Reservation reservation))
            {
                return ValidationResult.Failure("Invalid data type - expected Reservation");
            }

            var errors = new List<string>();

            // Validate Id
            if (string.IsNullOrWhiteSpace(reservation.Id))
                errors.Add("Reservation ID is required");

            // Validate GuestName
            if (string.IsNullOrWhiteSpace(reservation.GuestName))
                errors.Add("Guest name is required");

            // Validate CheckIn
            if (reservation.CheckIn < DateTime.Now.Date)
                errors.Add("Check-in date cannot be in the past");

            // Validate CheckOut
            if (reservation.CheckOut <= reservation.CheckIn)
                errors.Add("Check-out date must be after check-in date");

            return errors.Any()
                ? ValidationResult.Failure(errors.ToArray())
                : ValidationResult.Success();
        }
    }
}