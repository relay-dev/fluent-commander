using System;
using FluentCommander.Samples.Commands;
using FluentCommander.Samples.Framework;
using FluentCommander.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentCommander.Samples
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
            serviceCollection.AddSqlServerDatabaseCommander(_configuration);

            // Add other services needed to run the application
            serviceCollection.AddSingleton(_configuration);
            serviceCollection.AddTransient<ParameterlessSamples>();
            serviceCollection.AddTransient<BulkCopySamples>();
            serviceCollection.AddTransient<PaginationSamples>();
            serviceCollection.AddTransient<ScalarSamples>();
            serviceCollection.AddTransient<SqlNonQuerySamples>();
            serviceCollection.AddTransient<SqlQuerySamples>();
            serviceCollection.AddTransient<StoredProcedureSamples>();
            serviceCollection.AddTransient<DatabaseCommanderFactorySamples>();

            // Build the IServiceProvider
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
