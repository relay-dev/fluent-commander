using FluentCommander.Commands;
using FluentCommander.Utility;
using FluentCommander.Utility.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommander.Core.Impl
{
    public class Bootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            services.AddTransient<DatabaseCommandBuilder>();
            services.AddTransient<BulkCopyDatabaseCommand>();
            services.AddTransient<PaginationDatabaseCommand>();
            services.AddTransient(typeof(ScalarDatabaseCommand<>));
            services.AddTransient<SqlNonQueryDatabaseCommand>();
            services.AddTransient<SqlQueryDatabaseCommand>();
            services.AddTransient<StoredProcedureDatabaseCommand>();
            services.AddScoped<IAutoMapper, AutoMapper>();
            services.AddScoped<IDatabaseCommandFactory, DatabaseCommandFactory>();
            services.AddSingleton<IConnectionStringCollection, ConnectionStringFromConfigurationCollection>();

            return services;
        }
    }
}
