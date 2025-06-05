namespace DIRS21.Mapping.Validation
{
    /// <summary>
    /// Interface for validators that know which type they validate
    /// </summary>
    public interface IValidatorType
    {
        /// <summary>
        /// The type name this validator is responsible for
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Validate the provided data
        /// </summary>
        ValidationResult Validate(object data);
    }
}
