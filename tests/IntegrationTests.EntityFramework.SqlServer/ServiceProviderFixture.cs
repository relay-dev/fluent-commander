using System;
using FluentCommander.EntityFramework.SqlServer;
using Xunit;

namespace IntegrationTests.EntityFramework.SqlServer
{
    public class ServiceProviderFixture : IDisposable
    {
        public IServiceProvider ServiceProvider;

        public ServiceProviderFixture()
        {
            ServiceProvider = new Startup().ConfigureServices().UseEntityFrameworkSqlServerDatabaseCommander();
        }

        public void Dispose()
        {
            ServiceProvider = null;
        }
    }

    /// <summary>
    /// This is needed for xUnit
    /// </summary>
    [CollectionDefinition("Service Provider collection")]
    public class ServiceProviderCollection : ICollectionFixture<ServiceProviderFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
