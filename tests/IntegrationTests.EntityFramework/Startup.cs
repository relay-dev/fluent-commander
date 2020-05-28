using FluentCommander.Core.Impl;
using FluentCommander.EntityFramework;
using IntegrationTests.EntityFramework.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.EntityFramework
{
    public class Startup : Bootstrapper
    {
        public override void AddDatabaseCommander(ServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.AddDatabaseCommander();
        }

        protected override void ConfigureDomainServices(ServiceCollection serviceCollection, IConfiguration config)
        {
            var connectionStringCollection = new ConnectionStringCollection(config);

            serviceCollection.AddScoped<DatabaseCommanderDomainContext>();
            serviceCollection.AddScoped<DbContext, DatabaseCommanderDomainContext>();
            serviceCollection.AddSingleton(new SqlConnectionStringBuilder(connectionStringCollection.Get("DefaultConnection")));
        }
    }
}
