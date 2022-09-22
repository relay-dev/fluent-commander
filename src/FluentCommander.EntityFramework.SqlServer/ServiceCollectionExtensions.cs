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
            var options = new SqlServerCommanderOptions
            {
                Configuration = configuration
            };

            new SqlServerCommanderBootstrapper().Bootstrap(services, options);

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
