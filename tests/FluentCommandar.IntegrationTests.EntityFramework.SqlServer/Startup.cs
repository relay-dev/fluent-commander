using FluentCommander.EntityFramework.SqlServer;
using FluentCommander.IntegrationTests.EntityFramework.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommander.IntegrationTests.EntityFramework.SqlServer
{
    public class Startup : Bootstrapper
    {
        public override void AddDatabaseCommander(ServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.AddEntityFrameworkSqlServerDatabaseCommander(config);
        }

        protected override void ConfigureDomainServices(ServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.AddScoped<DatabaseCommanderDomainContext>();
            serviceCollection.AddScoped<DbContext, DatabaseCommanderDomainContext>();
        }
    }
}
