using DIRS21.Mapping.Core.Interfaces;
using DIRS21.Mapping.Validation;

namespace DIRS21.Mapping.Services
{
    /// <summary>
    /// Default validation service implementation
    /// </summary>
    public class ValidationService : IValidationService
    {
        private readonly Dictionary<string, IValidatorType> _validators =
            new Dictionary<string, IValidatorType>(StringComparer.OrdinalIgnoreCase);

        public void RegisterValidator(string typeName, IValidatorType validator)
        {
            _validators[typeName] = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public ValidationResult ValidateSource(object data, string sourceType)
        {
            if (_validators.TryGetValue(sourceType, out var validator))
            {
                return validator.Validate(data);
            }

            // If no validator registered, assume valid
            return ValidationResult.Success();
        }

        public ValidationResult ValidateTarget(object data, string targetType)
        {
            if (_validators.TryGetValue(targetType, out var validator))
            {
                return validator.Validate(data);
            }

            return ValidationResult.Success();
        }
    }
}