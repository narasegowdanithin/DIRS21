using Microsoft.Extensions.DependencyInjection;
using DIRS21.Mapping.Core.Interfaces;
using DIRS21.Mapping.Services;
using DIRS21.Mapping.Validation;
using DIRS21.Mapping.Core;

namespace DIRS21.Mapping.Extensions
{
    /// <summary>
    /// Extension methods for configuring DIRS21 Mapping in DI container
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add DIRS21 Mapping core services to the service collection
        /// </summary>
        public static IServiceCollection AddDIRS21Mapping(this IServiceCollection services)
        {
            // Register core services
            services.AddSingleton<IMapperRegistry, MapperRegistry>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IMapperFactory, MapperFactory>();
            services.AddScoped<MapHandler>();


            return services;
        }

        /// <summary>
        /// Register all mappers from specified assembly and register MapperInitializationService
        /// </summary>
        public static IServiceCollection AddMappersFromAssembly(
            this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var mapperTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract &&
                       typeof(IMapper).IsAssignableFrom(t) &&
                       t.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            foreach (var mapperType in mapperTypes)
            {
                services.AddTransient(typeof(IMapper), mapperType);
            }

            // Register MapperInitializationService to initialize registry on startup
            services.AddHostedService<MapperInitializationService>();

            return services;
        }

        /// <summary>
        /// Register validators from assembly
        /// </summary>
        public static IServiceCollection AddValidatorsFromAssembly(
            this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var validatorTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => !t.IsAbstract &&
                           typeof(IValidatorType).IsAssignableFrom(t) &&
                           t.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            foreach (var validatorType in validatorTypes)
            {
                services.AddTransient(typeof(IValidatorType), validatorType);
            }
            return services;
        }
    }
}
