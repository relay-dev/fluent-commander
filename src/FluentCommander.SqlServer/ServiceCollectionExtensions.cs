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
            var options = new SqlServerCommanderOptions
            {
                Configuration = configuration
            };

            new SqlServerCommanderBootstrapper().Bootstrap(services, options);

            services.AddScoped<IDatabaseCommanderFactory, SqlServerDatabaseCommanderFactory>();
            services.AddScoped<IDatabaseRequestHandlerFactory, SqlServerDatabaseRequestHandlerFactory>();
            services.AddTransient<ISqlServerConnectionProvider, SqlServerConnectionProvider>();

            return services;
        }

        public static IServiceCollection AddFluentCommander(this IServiceCollection services, Action<SqlServerCommanderOptions> options)
        {
            var optionsSet = new SqlServerCommanderOptions();

            options.Invoke(optionsSet);

            new SqlServerCommanderBootstrapper().Bootstrap(services, optionsSet);

            services.AddScoped<IDatabaseCommanderFactory, SqlServerDatabaseCommanderFactory>();
            services.AddScoped<IDatabaseRequestHandlerFactory, SqlServerDatabaseRequestHandlerFactory>();
            services.AddTransient<ISqlServerConnectionProvider, SqlServerConnectionProvider>();

            return services;
        }
    }
}
