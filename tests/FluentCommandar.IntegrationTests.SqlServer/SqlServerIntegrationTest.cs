using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace FluentCommander.IntegrationTests.SqlServer
{
    public class SqlServerIntegrationTest<TSUT> : IntegrationTest<TSUT>
    {
        private readonly ServiceProviderFixture _serviceProviderFixture;

        public SqlServerIntegrationTest(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(output)
        {
            _serviceProviderFixture = serviceProviderFixture;

            Init();
        }

        protected override TService ResolveService<TService>()
        {
            return (TService)_serviceProviderFixture.ServiceProvider.GetRequiredService(typeof(TService));
        }
    }
}
