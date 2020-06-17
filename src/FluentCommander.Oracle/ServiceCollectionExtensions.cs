using FluentCommander.Core.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace FluentCommander.Oracle
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOracleDatabaseCommander(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IDatabaseCommanderFactory, OracleDatabaseCommanderFactory>();

            var connectionStringCollection = new ConnectionStringCollection(config);

            if (connectionStringCollection.ConnectionStringNames.Contains("DefaultConnection"))
            {
                services.AddSingleton(new OracleConnectionStringBuilder(connectionStringCollection.Get("DefaultConnection")));
                services.AddTransient<IDatabaseCommander, OracleDatabaseCommander>();
            }

            return services.AddDatabaseCommander();
        }
    }
}
