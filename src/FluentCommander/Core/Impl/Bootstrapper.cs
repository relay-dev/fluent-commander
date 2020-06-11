using FluentCommander.BulkCopy;
using FluentCommander.Core.Utility;
using FluentCommander.Core.Utility.Impl;
using FluentCommander.Pagination;
using FluentCommander.Scalar;
using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
using FluentCommander.StoredProcedure;
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
            services.AddTransient(typeof(StoredProcedureCommand<>));
            services.AddTransient<IRequestValidator<BulkCopyRequest>, BulkCopyRequestValidator>();
            services.AddTransient<IRequestValidator<PaginationRequest>, PaginationRequestValidator>();
            services.AddScoped<IAutoMapper, AutoMapper>();
            services.AddScoped<IDatabaseCommandFactory, DatabaseCommandFactory>();
            services.AddSingleton<IConnectionStringCollection, ConnectionStringCollection>();

            return services;
        }
    }
}
