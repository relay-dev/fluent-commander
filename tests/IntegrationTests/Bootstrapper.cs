using FluentCommander.Samples.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace FluentCommander.IntegrationTests
{
    public abstract class Bootstrapper
    {
        public abstract void AddDatabaseCommander(ServiceCollection serviceCollection, IConfiguration config);

        public IServiceProvider ConfigureServices()
        {
            // Build configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Initialize a ServiceCollection
            var serviceCollection = new ServiceCollection();

            // Add the DatabaseCommander framework
            AddDatabaseCommander(serviceCollection, config);

            // Invoke the configuration for subclasses to add services
            ConfigureDomainServices(serviceCollection, config);

            // Add other services needed to run the application
            serviceCollection.AddSingleton<IConfiguration>(config);

            // Setup the database if it's not initialized
            Initialize(config);

            // Build the IServiceProvider
            return serviceCollection.BuildServiceProvider();
        }

        protected virtual void ConfigureDomainServices(ServiceCollection serviceCollection, IConfiguration config)
        {
            
        }

        private void Initialize(IConfiguration config)
        {
             var databaseService = new DatabaseService(config);

             if (!databaseService.IsInitialized())
             {
                 databaseService.SetupDatabase();
             }
        }
    }
}
