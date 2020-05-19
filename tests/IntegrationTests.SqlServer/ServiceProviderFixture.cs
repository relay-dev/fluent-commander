using System;
using Xunit;

namespace IntegrationTests.SqlServer
{
    public class ServiceProviderFixture : IDisposable
    {
        public IServiceProvider ServiceProvider;

        public ServiceProviderFixture()
        {
            ServiceProvider = new Startup().ConfigureServices();
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
