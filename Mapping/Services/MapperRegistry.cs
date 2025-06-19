using DIRS21.Mapping.Core.Interfaces;

namespace DIRS21.Mapping.Services
{
    /// <summary>
    /// Default implementation of mapper registry
    /// </summary>
    public class MapperRegistry : IMapperRegistry
    {
        private readonly Dictionary<string, IMapper> _mappers =
         new Dictionary<string, IMapper>(StringComparer.OrdinalIgnoreCase);

        public void Register(IMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            var key = GetMapperKey(mapper.SourceType, mapper.TargetType);
            _mappers[key] = mapper;
        }

        public void Register<TMapper>() where TMapper : IMapper, new()
        {
            Register(new TMapper());
        }

        //private readonly Dictionary<string, IMapper> _mappers; //For DI
        //public MapperRegistry(IEnumerable<IMapper> mappers)
        //{
        //    if (mappers == null)
        //        throw new ArgumentNullException(nameof(mappers));

        //    _mappers = mappers.ToDictionary(
        //        m => $"{m.SourceType}|{m.TargetType}",
        //        m => m,
        //        StringComparer.OrdinalIgnoreCase);
        //}

        public IMapper GetMapper(string sourceType, string targetType)
        {
            var key = GetMapperKey(sourceType, targetType);
            return _mappers.TryGetValue(key, out var mapper) ? mapper : null;
        }

        public IEnumerable<(string SourceType, string TargetType)> GetRegisteredMappings()
        {
            return _mappers.Values.Select(m => (m.SourceType, m.TargetType));
        }

        private string GetMapperKey(string sourceType, string targetType)
        {
            return $"{sourceType}|{targetType}";
        }
    }
}