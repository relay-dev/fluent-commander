using FluentCommander.Core;
using FluentCommander.Core.Impl;
using FluentCommander.Database.Commands;
using FluentCommander.Database.Utility;
using FluentCommander.Database.Utility.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace FluentCommander.Database.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseCommander(this IServiceCollection services, IConfiguration config)
        {
            var connectionStringProvider = new ConnectionStringFromConfigurationProvider(config);

            if (connectionStringProvider.ConnectionStringNames.Contains("DefaultConnection"))
            {
                services.AddSingleton(new SqlConnectionStringBuilder(connectionStringProvider.Get("DefaultConnection")));
                services.AddTransient<IDatabaseCommander, SqlServerDatabaseCommander>();
            }

            services.AddTransient<DatabaseCommandBuilder>();
            services.AddTransient<BulkCopyDatabaseCommand>();
            services.AddTransient<PaginationDatabaseCommand>();
            services.AddTransient<SqlNonQueryDatabaseCommand>();
            services.AddTransient<SqlQueryDatabaseCommand>();
            services.AddTransient<StoredProcedureDatabaseCommand>();
            services.AddScoped<IAutoMapper, AutoMapper>();
            services.AddScoped<IDatabaseCommandFactory, DatabaseCommandFactory>();
            services.AddScoped<IDatabaseCommanderFactory, SqlServerDatabaseCommanderFactory>();
            services.AddSingleton<IConnectionStringProvider, ConnectionStringFromConfigurationProvider>();

            return services;
        }
    }
}
