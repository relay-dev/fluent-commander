using FluentCommander.Core.Impl;
using FluentCommander.SqlServer.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommander.SqlServer.Bootstrap
{
    public class SqlServerCommanderBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services, SqlServerCommanderOptions options)
        {
            new CommanderBootstrapper().Bootstrap(services);

            services.AddTransient<SqlServerBulkCopyCommand>();
            services.AddTransient<SqlServerPaginationCommand>();
            services.AddTransient(typeof(SqlServerScalarCommand<>));
            services.AddTransient<SqlServerSqlNonQueryCommand>();
            services.AddTransient<SqlServerSqlQueryCommand>();
            services.AddTransient<SqlServerStoredProcedureCommand>();
            services.AddTransient<ISqlServerCommandExecutor, SqlServerCommandExecutor>();

            if (string.IsNullOrWhiteSpace(options.ConnectionString) && options.Configuration != null)
            {
                var connectionStringCollection = new ConnectionStringCollection(options.Configuration);

                if (connectionStringCollection.ConnectionStringNames.Contains("DefaultConnection"))
                {
                    options.ConnectionString = connectionStringCollection.Get("DefaultConnection");
                }
            }

            if (!string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                services.AddSingleton(new SqlConnectionStringBuilder(options.ConnectionString));
                services.AddTransient<IDatabaseCommander, SqlServerDatabaseCommander>();
            }

            return services;
        }
    }
}
