using FluentCommander.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentCommander.EntityFramework.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkSqlServerDatabaseCommander(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseCommander, EntityFrameworkSqlServerDatabaseCommander>();
            services.AddTransient<ISqlServerConnectionProvider, EntityFrameworkConnectionProvider>();

            services.AddSqlServerDatabaseCommands();

            return services.AddEntityFrameworkDatabaseCommander();
        }

        public static IServiceProvider UseEntityFrameworkSqlServerDatabaseCommander(this IServiceProvider serviceProvider)
        {
            return serviceProvider.UseEntityFrameworkDatabaseCommander();
        }
    }
}
