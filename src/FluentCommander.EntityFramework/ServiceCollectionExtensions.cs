using FluentCommander.Core.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentCommander.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseCommander(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseCommander, EntityFrameworkDatabaseCommander>();
            services.AddTransient<DbContextCommandBuilder>();

            return new Bootstrapper().Bootstrap(services);
        }

        public static IServiceProvider UseAddDatabaseCommander(this IServiceProvider serviceProvider)
        {
            DbContextExtensions.DatabaseCommandBuilder = serviceProvider.GetRequiredService<DbContextCommandBuilder>();

            return serviceProvider;
        }
    }
}
