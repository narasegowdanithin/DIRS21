namespace DIRS21.Mapping.Core.Interfaces
{
    /// <summary>
    /// Interface for all mapper implementations
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Source type identifier (e.g., "Model.Reservation")
        /// </summary>
        string SourceType { get; }

        /// <summary>
        /// Target type identifier (e.g., "Google.Reservation")
        /// </summary>
        string TargetType { get; }

        /// <summary>
        /// Performs the mapping from source to target
        /// </summary>
        object Map(object source);
    }
}