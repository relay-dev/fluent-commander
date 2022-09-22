using FluentCommander.SqlServer.Bootstrap;
using FluentCommander.SqlServer.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommander.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentCommander(this IServiceCollection services, IConfiguration configuration, string connectionString = null)
        {
            new SqlServerCommanderBootstrapper().Bootstrap(services, configuration, connectionString);

            services.AddScoped<IDatabaseCommanderFactory, SqlServerDatabaseCommanderFactory>();
            services.AddTransient<ISqlServerConnectionProvider, SqlServerConnectionProvider>();

            return services;
        }
    }
}
