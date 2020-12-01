using FluentCommander.SqlServer;
using FluentCommander.SqlServer.Bootstrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentCommander.EntityFramework.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkSqlServerDatabaseCommander(this IServiceCollection services, IConfiguration configuration)
        {
            new SqlServerCommanderBootstrapper().Bootstrap(services, configuration);

            services.AddTransient<IDatabaseCommander, EntityFrameworkSqlServerDatabaseCommander>();
            services.AddTransient<IDatabaseCommanderFactory, SqlServerDatabaseCommanderFactory>();
            services.AddTransient<ISqlServerConnectionProvider, EntityFrameworkConnectionProvider>();
            services.AddScoped<IDatabaseEntityCommanderFactory, EntityFrameworkSqlServerDatabaseCommanderFactory>();

            return services.AddEntityFrameworkDatabaseCommander();
        }

        public static IServiceProvider UseEntityFrameworkSqlServerDatabaseCommander(this IServiceProvider serviceProvider)
        {
            return serviceProvider.UseEntityFrameworkDatabaseCommander();
        }
    }
}
