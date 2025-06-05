namespace DIRS21.Mapping.Validation
{
    /// <summary>
    /// Result of a validation operation
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; }
        public List<string> Errors { get; }

        private ValidationResult(bool isValid, List<string> errors = null)
        {
            IsValid = isValid;
            Errors = errors ?? new List<string>();
        }

        public static ValidationResult Success() => new ValidationResult(true);

        public static ValidationResult Failure(params string[] errors) =>
            new ValidationResult(false, errors.ToList());
    }
}