using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DIRS21.Mapping.Core.Interfaces;
using DIRS21.Mapping.Validation;

namespace DIRS21.Mapping.Services
{
    /// <summary>
    /// Background service that initializes all mappers and validators on startup
    /// </summary>
    public class MapperInitializationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MapperInitializationService> _logger;

        public MapperInitializationService(
            IServiceProvider serviceProvider,
            ILogger<MapperInitializationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Initialize Mappers
                InitializeMappers(scope);

                // Initialize Validators
                InitializeValidators(scope);
            }

            _logger.LogInformation("MapperInitializationService completed initialization");
            return Task.CompletedTask;
        }

        private void InitializeMappers(IServiceScope scope)
        {
            var registry = scope.ServiceProvider.GetRequiredService<IMapperRegistry>();
            var mappers = scope.ServiceProvider.GetServices<IMapper>().ToList();

            if (mappers.Count == 0)
            {
                _logger.LogWarning("No mappers were found to register.");
            }
            else
            {
                _logger.LogInformation($"Found {mappers.Count} mappers to register");
            }

            foreach (var mapper in mappers)
            {
                registry.Register(mapper);
                _logger.LogInformation(
                    "Registered mapper: {SourceType} -> {TargetType}",
                    mapper.SourceType,
                    mapper.TargetType);
            }
        }

        private void InitializeValidators(IServiceScope scope)
        {
            var validationService = scope.ServiceProvider.GetRequiredService<IValidationService>();
            if (validationService == null)
            {
                _logger.LogWarning("ValidationService not found or not of type ValidationService");
                return;
            }

            // Register ITypedValidators
            var typedValidators = scope.ServiceProvider.GetServices<IValidatorType>().ToList();
            if (typedValidators.Count > 0)
            {
                _logger.LogInformation($"Found {typedValidators.Count} typed validators to register");

                foreach (var validator in typedValidators)
                {
                    validationService.RegisterValidator(validator.TypeName, validator);
                    _logger.LogInformation(
                        "Registered validator for type: {TypeName}",
                        validator.TypeName);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}