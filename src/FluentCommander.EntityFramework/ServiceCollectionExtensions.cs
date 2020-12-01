using FluentCommander.Core.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentCommander.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkDatabaseCommander(this IServiceCollection services)
        {
            new CommanderBootstrapper().Bootstrap(services);

            services.AddTransient<DbContextCommandBuilder>();

            return services;
        }

        public static IServiceProvider UseEntityFrameworkDatabaseCommander(this IServiceProvider serviceProvider)
        {
            DbContextExtensions.ServiceProvider = serviceProvider;

            return serviceProvider;
        }
    }
}
