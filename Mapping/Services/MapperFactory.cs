using DIRS21.Mapping.Core.Interfaces;

namespace DIRS21.Mapping.Services
{
    /// <summary>
    /// Default mapper factory implementation
    /// </summary>
    public class MapperFactory : IMapperFactory
    {
        private readonly IMapperRegistry _registry;

        public MapperFactory(IMapperRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public IMapper GetMapper(string sourceType, string targetType)
        {
            return _registry.GetMapper(sourceType, targetType);
        }
    }
}