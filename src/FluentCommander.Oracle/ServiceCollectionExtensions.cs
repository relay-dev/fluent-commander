using FluentCommander.Core.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using System;

namespace FluentCommander.Oracle
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentCommander(this IServiceCollection services, IConfiguration configuration)
        {
            new CommanderBootstrapper().Bootstrap(services);

            services.AddScoped<IDatabaseCommanderFactory, OracleDatabaseCommanderFactory>();

            var connectionStringCollection = new ConnectionStringCollection(configuration);

            if (connectionStringCollection.ConnectionStringNames.Contains("DefaultConnection"))
            {
                services.AddSingleton(new OracleConnectionStringBuilder(connectionStringCollection.Get("DefaultConnection")));
                services.AddTransient<IDatabaseCommander, OracleDatabaseCommander>();
            }

            return services;
        }

        public static IServiceCollection AddFluentCommander(this IServiceCollection services, Action<OracleCommanderOptions> options)
        {
            var optionsSet = new OracleCommanderOptions();

            options.Invoke(optionsSet);

            new CommanderBootstrapper().Bootstrap(services);

            services.AddScoped<IDatabaseCommanderFactory, OracleDatabaseCommanderFactory>();

            if (string.IsNullOrWhiteSpace(optionsSet.ConnectionString) && optionsSet.Configuration != null)
            {
                var connectionStringCollection = new ConnectionStringCollection(optionsSet.Configuration);

                if (connectionStringCollection.ConnectionStringNames.Contains("DefaultConnection"))
                {
                    optionsSet.ConnectionString = connectionStringCollection.Get("DefaultConnection");
                }
            }

            if (!string.IsNullOrWhiteSpace(optionsSet.ConnectionString))
            {
                services.AddSingleton(new OracleConnectionStringBuilder(optionsSet.ConnectionString));
                services.AddTransient<IDatabaseCommander, OracleDatabaseCommander>();
            }

            return services;
        }
    }
}