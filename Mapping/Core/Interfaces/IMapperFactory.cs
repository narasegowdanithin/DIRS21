namespace DIRS21.Mapping.Core.Interfaces
{
    /// <summary>
    /// Factory for creating mapper instances
    /// </summary>
    public interface IMapperFactory
    {
        /// <summary>
        /// Get mapper for specific source and target types
        /// </summary>
        IMapper GetMapper(string sourceType, string targetType);
    }
}