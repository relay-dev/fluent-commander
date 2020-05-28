using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace IntegrationTests.EntityFramework
{
    public class EntityFrameworkIntegrationTest<TSUT> : IntegrationTest<TSUT>
    {
        private readonly ServiceProviderFixture _serviceProviderFixture;

        public EntityFrameworkIntegrationTest(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
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
