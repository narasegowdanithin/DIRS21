using DIRS21.Mapping.Models.External.Google;

namespace DIRS21.Mapping.Validation.Validator
{
    /// <summary>
    /// Validator for Google Reservation model
    /// </summary>
    public class GoogleReservationValidator : IValidatorType
    {
        public string TypeName => "Google.Reservation";

        public ValidationResult Validate(object data)
        {
            if (!(data is GoogleReservation googleReservation))
            {
                return ValidationResult.Failure("Invalid data type - expected GoogleReservation");
            }

            var errors = new List<string>();

            // Validate ReservationId
            if (string.IsNullOrWhiteSpace(googleReservation.ReservationId)) 
            { 
                errors.Add("Reservation ID is required");
            }
            // Validate Guest
            if (string.IsNullOrWhiteSpace(googleReservation.Guest))
            {
                errors.Add("Guest name is required");
            }
                

            // Validate StartDate
            if (string.IsNullOrWhiteSpace(googleReservation.StartDate))
            {
                errors.Add("Start date is required");
            }
            else
            {
                if (!DateTime.TryParse(googleReservation.StartDate, out var startDate))
                {
                    errors.Add("Start date must be a valid date in format yyyy-MM-dd");
                }
                else if (startDate < DateTime.Now.Date)
                {
                    errors.Add("Start date cannot be in the past");
                }
            }

            // Validate EndDate
            if (string.IsNullOrWhiteSpace(googleReservation.EndDate))
            {
                errors.Add("End date is required");
            }
            else
            {
                if (!DateTime.TryParse(googleReservation.EndDate, out var endDate))
                {
                    errors.Add("End date must be a valid date in format yyyy-MM-dd");
                }
                else if (!string.IsNullOrWhiteSpace(googleReservation.StartDate) &&
                         DateTime.TryParse(googleReservation.StartDate, out var startDate))
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