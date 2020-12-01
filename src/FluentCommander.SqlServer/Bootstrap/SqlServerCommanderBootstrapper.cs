using FluentCommander.Core.Impl;
using FluentCommander.SqlServer.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommander.SqlServer.Bootstrap
{
    public class SqlServerCommanderBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services, IConfiguration configuration)
        {
            new CommanderBootstrapper().Bootstrap(services);

            services.AddTransient<SqlServerBulkCopyCommand>();
            services.AddTransient<SqlServerPaginationCommand>();
            services.AddTransient(typeof(SqlServerScalarCommand<>));
            services.AddTransient<SqlServerSqlNonQueryCommand>();
            services.AddTransient<SqlServerSqlQueryCommand>();
            services.AddTransient<SqlServerStoredProcedureCommand>();
            services.AddTransient<ISqlServerCommandExecutor, SqlServerCommandExecutor>();

            var connectionStringCollection = new ConnectionStringCollection(configuration);

            if (connectionStringCollection.ConnectionStringNames.Contains("DefaultConnection"))
            {
                services.AddSingleton(new SqlConnectionStringBuilder(connectionStringCollection.Get("DefaultConnection")));
                services.AddTransient<IDatabaseCommander, SqlServerDatabaseCommander>();
            }

            return services;
        }
    }
}
