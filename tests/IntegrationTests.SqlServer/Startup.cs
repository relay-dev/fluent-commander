using FluentCommander.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.SqlServer
{
    public class Startup : Bootstrapper
    {
        public override void AddDatabaseCommander(ServiceCollection serviceCollection, IConfiguration config)
        {
            // Add the DatabaseCommander framework
            serviceCollection.AddDatabaseCommander(config);
        }
    }
}
