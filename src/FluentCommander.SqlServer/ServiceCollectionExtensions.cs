using FluentCommander.SqlServer.Bootstrap;
using FluentCommander.SqlServer.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentCommander.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentCommander(this IServiceCollection services, IConfiguration configuration)
        {
            new SqlServerCommanderBootstrapper().Bootstrap(services, configuration);

            services.AddScoped<IDatabaseCommanderFactory, SqlServerDatabaseCommanderFactory>();
            services.AddTransient<ISqlServerConnectionProvider, SqlServerConnectionProvider>();

            return services;
        }

        public static IServiceCollection AddFluentCommander(this IServiceCollection services, Action<SqlServerCommanderOptions> options)
        {
            var optionsSet = new SqlServerCommanderOptions();

            options.Invoke(optionsSet);

            new SqlServerCommanderBootstrapper().Bootstrap(services, optionsSet.Configuration, optionsSet.ConnectionString);

            services.AddScoped<IDatabaseCommanderFactory, SqlServerDatabaseCommanderFactory>();
            services.AddTransient<ISqlServerConnectionProvider, SqlServerConnectionProvider>();

            return services;
        }
    }
}
