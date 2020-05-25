using FluentCommander.Core.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using FluentCommander.Database.Core.Impl;

namespace FluentCommander.Database.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseCommander(this IServiceCollection services, IConfiguration config)
        {
            new Bootstrapper().Bootstrap(services);

            services.AddScoped<IDatabaseCommanderFactory, SqlServerDatabaseCommanderFactory>();

            var connectionStringProvider = new ConnectionStringFromConfigurationProvider(config);

            if (connectionStringProvider.ConnectionStringNames.Contains("DefaultConnection"))
            {
                services.AddSingleton(new SqlConnectionStringBuilder(connectionStringProvider.Get("DefaultConnection")));
                services.AddTransient<IDatabaseCommander, SqlServerDatabaseCommander>();
            }

            return services;
        }
    }
}
