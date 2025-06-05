using DIRS21.Mapping.Core.Exceptions;
using DIRS21.Mapping.Core.Interfaces;

namespace DIRS21.Mapping.Core
{
    /// <summary>
    /// Core mapping handler for transforming data between formats
    /// </summary>
    public class MapHandler
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IValidationService _validationService;
        private readonly IMapperFactory _mapperFactory;

        public MapHandler(
            IMapperRegistry mapperRegistry,
            IValidationService validationService,
            IMapperFactory mapperFactory)
        {
            _mapperRegistry = mapperRegistry ?? throw new ArgumentNullException(nameof(mapperRegistry));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _mapperFactory = mapperFactory ?? throw new ArgumentNullException(nameof(mapperFactory));
        }

        /// <summary>
        /// Maps data from source type to target type
        /// </summary>
        public object Map(object data, string sourceType, string targetType)
        {
            // Input validation
            if (data == null) 
            { 
                throw new ArgumentNullException(nameof(data));
            }
            if (string.IsNullOrWhiteSpace(sourceType)) 
            { 
                throw new ArgumentException("Source type cannot be null or empty", nameof(sourceType));
            }
            if (string.IsNullOrWhiteSpace(targetType)) 
            { 
                throw new ArgumentException("Target type cannot be null or empty", nameof(targetType));
            }
            try
            {
                // Validate source data
                var validationResult = _validationService.ValidateSource(data, sourceType);
                if (!validationResult.IsValid)
                {
                    throw new MappingValidationException(
                        $"Source validation failed: {string.Join(", ", validationResult.Errors)}");
                }

                // Get mapper
                var mapper = _mapperFactory.GetMapper(sourceType, targetType);
                if (mapper == null)
                {
                    throw new MappingNotFoundException(
                        $"No mapper found for {sourceType} -> {targetType}");
                }

                // Perform mapping
                var result = mapper.Map(data);

                // Validate target data
                var targetValidationResult = _validationService.ValidateTarget(result, targetType);
                if (!targetValidationResult.IsValid)
                {
                    throw new MappingValidationException(
                        $"Target validation failed: {string.Join(", ", targetValidationResult.Errors)}");
                }

                return result;
            }
            catch (Exception ex) when (!(ex is MappingException))
            {
                throw new MappingException(
                    $"Error mapping from {sourceType} to {targetType}", ex);
            }
        }

        /// <summary>
        /// Asynchronously maps data from source type to target type
        /// </summary>
        public async Task<object> MapAsync(object data, string sourceType, string targetType)
        {
            return await Task.Run(() => Map(data, sourceType, targetType));
        }
    }
}