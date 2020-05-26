using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentCommander.Core.Impl;
using Oracle.ManagedDataAccess.Client;

namespace FluentCommander.Oracle
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseCommander(this IServiceCollection services, IConfiguration config)
        {
            new Bootstrapper().Bootstrap(services);

            services.AddScoped<IDatabaseCommanderFactory, OracleDatabaseCommanderFactory>();

            var connectionStringCollection = new ConnectionStringFromConfigurationCollection(config);

            if (connectionStringCollection.ConnectionStringNames.Contains("DefaultConnection"))
            {
                services.AddSingleton(new OracleConnectionStringBuilder(connectionStringCollection.Get("DefaultConnection")));
                services.AddTransient<IDatabaseCommander, OracleDatabaseCommander>();
            }

            return services;
        }
    }
}
