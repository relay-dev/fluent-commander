using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentCommander.Core.Impl;
using Oracle.ManagedDataAccess.Client;

namespace FluentCommander.Oracle
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentCommander(this IServiceCollection services, IConfiguration configuration, string connectionString = null)
        {
            new CommanderBootstrapper().Bootstrap(services);

            services.AddScoped<IDatabaseCommanderFactory, OracleDatabaseCommanderFactory>();

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var connectionStringCollection = new ConnectionStringCollection(configuration);

                if (connectionStringCollection.ConnectionStringNames.Contains("DefaultConnection"))
                {
                    connectionString = connectionStringCollection.Get("DefaultConnection");
                }
            }

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddSingleton(new OracleConnectionStringBuilder(connectionString));
                services.AddTransient<IDatabaseCommander, OracleDatabaseCommander>();
            }

            return services;
        }
    }
}
