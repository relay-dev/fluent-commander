using FluentCommander.BulkCopy;
using FluentCommander.Core.Impl;
using FluentCommander.SqlServer.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommander.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDatabaseCommander(this IServiceCollection services, IConfiguration config)
        {
            new Bootstrapper().Bootstrap(services);

            services.AddScoped<IDatabaseCommanderFactory, SqlServerDatabaseCommanderFactory>();
            services.AddTransient<ISqlServerConnectionProvider, SqlServerConnectionProvider>();
            services.AddTransient<IBulkCopyCommand, SqlServerBulkCopyCommand>();

            var connectionStringCollection = new ConnectionStringCollection(config);

            if (connectionStringCollection.ConnectionStringNames.Contains("DefaultConnection"))
            {
                services.AddSingleton(new SqlConnectionStringBuilder(connectionStringCollection.Get("DefaultConnection")));
                services.AddTransient<IDatabaseCommander, SqlServerDatabaseCommander>();
            }

            return services.AddSqlServerDatabaseCommands();
        }

        public static IServiceCollection AddSqlServerDatabaseCommands(this IServiceCollection services)
        {
            services.AddTransient<IBulkCopyCommand, SqlServerBulkCopyCommand>();

            return services;
        }
    }
}
