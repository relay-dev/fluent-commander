﻿using FluentCommander.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommander.IntegrationTests.SqlServer
{
    public class Startup : Bootstrapper
    {
        public override void AddDatabaseCommander(ServiceCollection serviceCollection, IConfiguration config)
        {
            // Add the DatabaseCommander framework
            serviceCollection.AddSqlServerDatabaseCommander(config);
        }
    }
}