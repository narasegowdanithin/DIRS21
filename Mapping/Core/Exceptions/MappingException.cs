namespace DIRS21.Mapping.Core.Exceptions
{
    /// <summary>
    /// Base exception for all mapping errors
    /// </summary>
    public class MappingException : Exception
    {
        public MappingException(string message) : base(message) { }
        public MappingException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when no mapper is found for the requested transformation
    /// </summary>
    public class MappingNotFoundException : MappingException
    {
        public MappingNotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when validation fails
    /// </summary>
    public class MappingValidationException : MappingException
    {
        public MappingValidationException(string message) : base(message) { }
    }
}