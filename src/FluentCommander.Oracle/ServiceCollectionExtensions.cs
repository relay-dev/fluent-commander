using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentCommander.Core.Impl;
using Oracle.ManagedDataAccess.Client;

namespace FluentCommander.Oracle
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentCommander(this IServiceCollection services, IConfiguration config)
        {
            new CommanderBootstrapper().Bootstrap(services);

            services.AddScoped<IDatabaseCommanderFactory, OracleDatabaseCommanderFactory>();

            var connectionStringCollection = new ConnectionStringCollection(config);

            if (connectionStringCollection.ConnectionStringNames.Contains("DefaultConnection"))
            {
                services.AddSingleton(new OracleConnectionStringBuilder(connectionStringCollection.Get("DefaultConnection")));
                services.AddTransient<IDatabaseCommander, OracleDatabaseCommander>();
            }

            return services;
        }
    }
}
