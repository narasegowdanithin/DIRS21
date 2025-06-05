namespace DIRS21.Mapping.Core.Interfaces
{
    /// <summary>
    /// Registry for managing mapper registrations
    /// </summary>
    public interface IMapperRegistry
    {
        /// <summary>
        /// Register a mapper instance
        /// </summary>
        void Register(IMapper mapper);

        /// <summary>
        /// Register a mapper by type
        /// </summary>
        void Register<TMapper>() where TMapper : IMapper, new();

        /// <summary>
        /// Get mapper for specific source and target types
        /// </summary>
        IMapper GetMapper(string sourceType, string targetType);

        /// <summary>
        /// Get all registered mappings
        /// </summary>
        IEnumerable<(string SourceType, string TargetType)> GetRegisteredMappings();
    }
}