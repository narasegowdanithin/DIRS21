using DIRS21.Mapping.Validation;

namespace DIRS21.Mapping.Core.Interfaces
{
    /// <summary>
    /// Service for validating data before and after mapping
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Validate source data before mapping
        /// </summary>
        ValidationResult Validate(object data, string sourceType);

        /// <summary>
        /// Validate target data after mapping
        /// </summary>
        //ValidationResult ValidateTarget(object data, string targetType);


        /// <summary>
        /// Register Validator by type 
        /// </summary>
        void RegisterValidator(string typeName, IValidatorType validator);
    }
}