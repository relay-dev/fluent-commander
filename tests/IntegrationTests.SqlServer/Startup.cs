using FluentCommander.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace IntegrationTests.SqlServer
{
    public class Startup
    {
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
            serviceCollection.AddDatabaseCommander(config);

            // Add other services needed to run the application
            serviceCollection.AddSingleton<IConfiguration>(config);

            // Build the IServiceProvider
            return serviceCollection.BuildServiceProvider();
        }
    }
}
