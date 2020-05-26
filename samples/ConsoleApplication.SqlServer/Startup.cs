using ConsoleApplication.SqlServer.Samples;
using FluentCommander.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ConsoleApplication.SqlServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider ConfigureServices()
        {
            // Initialize a ServiceCollection and add logging
            var serviceCollection = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Debug);
                    builder.AddConsole();
                });

            // Add the DatabaseCommander framework
            serviceCollection.AddDatabaseCommander(_configuration);

            // Add other services needed to run the application
            serviceCollection.AddSingleton(_configuration);
            serviceCollection.AddTransient<BasicCommandSamples>();
            serviceCollection.AddTransient<BulkCopyCommandSamples>();
            serviceCollection.AddTransient<PaginationCommandSamples>();
            serviceCollection.AddTransient<ScalarCommandSamples>();
            serviceCollection.AddTransient<SqlNonQueryCommandSamples>();
            serviceCollection.AddTransient<SqlQueryCommandSamples>();
            serviceCollection.AddTransient<StoredProcedureCommandSamples>();

            // Build the IServiceProvider
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
