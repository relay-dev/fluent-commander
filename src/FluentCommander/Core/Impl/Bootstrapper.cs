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
            services.AddTransient<BulkCopyCommand>();
            services.AddTransient<PaginationCommand>();
            services.AddTransient(typeof(ScalarCommand<>));
            services.AddTransient<SqlNonQueryCommand>();
            services.AddTransient<SqlQueryCommand>();
            services.AddTransient<StoredProcedureCommand>();
            services.AddScoped<IAutoMapper, AutoMapper>();
            services.AddScoped<IDatabaseCommandFactory, DatabaseCommandFactory>();
            services.AddSingleton<IConnectionStringCollection, ConnectionStringCollection>();

            return services;
        }
    }
}
