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

        //private readonly Dictionary<string, IValidatorType> _validators; For DI

        //public ValidationService(IEnumerable<IValidatorType> validators)
        //{
        //    if (validators == null)
        //        throw new ArgumentNullException(nameof(validators));

        //    _validators = validators.ToDictionary(
        //        v => v.TypeName,
        //        v => v,
        //        StringComparer.OrdinalIgnoreCase);
        //}

        public ValidationResult Validate(object data, string type)
        {
            if (_validators.TryGetValue(type, out var validator))
            {
                return validator.Validate(data);
            }

            // If no validator registered, assume valid
            return ValidationResult.Success();
        }

    }
}