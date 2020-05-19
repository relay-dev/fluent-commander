using System.Data.SqlClient;
using FluentCommander.Core;
using FluentCommander.Core.Impl;
using FluentCommander.Database.Commands;
using FluentCommander.Database.Utility;
using FluentCommander.Database.Utility.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommander.Database.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseCommander(this IServiceCollection services, IConfiguration config)
        {
            string defaultConnection = config.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrEmpty(defaultConnection))
            {
                services.AddTransient<IDatabaseCommander, SqlServerDatabaseCommander>();
                services.AddSingleton(new SqlConnectionStringBuilder(defaultConnection));
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
