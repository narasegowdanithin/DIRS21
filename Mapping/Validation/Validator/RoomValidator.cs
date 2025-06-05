using DIRS21.Mapping.Models.Internal;

namespace DIRS21.Mapping.Validation.Validators
{
    /// <summary>
    /// Validator for DIRS21 Room model
    /// </summary>
    public class RoomValidator : IValidatorType
    {
        public string TypeName => "Model.Room";

        public ValidationResult Validate(object data)
        {
            if (!(data is Room room))
            {
                return ValidationResult.Failure("Invalid data type - expected Room");
            }

            var errors = new List<string>();

            // Validate RoomId
            if (string.IsNullOrWhiteSpace(room.RoomId))
                errors.Add("RoomId is required");

            // Validate Type
            if (string.IsNullOrWhiteSpace(room.Type))
                errors.Add("Room Type is required");

            // Validate Price
            if (room.Price < 0)
                errors.Add("Price cannot be negative");

            return errors.Any() 
                ? ValidationResult.Failure(errors.ToArray()) 
                : ValidationResult.Success();
        }
    }
}