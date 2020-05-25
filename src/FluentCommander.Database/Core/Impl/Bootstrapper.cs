using FluentCommander.Core;
using FluentCommander.Core.Impl;
using FluentCommander.Database.Commands;
using FluentCommander.Database.Utility;
using FluentCommander.Database.Utility.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommander.Database.Core.Impl
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
            services.AddSingleton<IConnectionStringProvider, ConnectionStringFromConfigurationProvider>();

            return services;
        }
    }
}
