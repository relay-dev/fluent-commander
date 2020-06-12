using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace IntegrationTests.EntityFramework.SqlServer
{
    public class EntityFrameworkSqlServerIntegrationTest<TSUT> : IntegrationTest<TSUT>
    {
        private readonly ServiceProviderFixture _serviceProviderFixture;

        public EntityFrameworkSqlServerIntegrationTest(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
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
