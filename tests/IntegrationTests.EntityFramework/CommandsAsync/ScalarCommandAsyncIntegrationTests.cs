using FluentCommander.EntityFramework;
using IntegrationTests.EntityFramework.Entities;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.EntityFramework.CommandsAsync
{
    [Collection("Service Provider collection")]
    public class ScalarCommandAsyncIntegrationTests : EntityFrameworkIntegrationTest<DatabaseCommanderDomainContext>
    {
        public ScalarCommandAsyncIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public async Task ExecuteScalarCommandAsync_WithInputParameters_ShouldReturnResult()
        {
            // Arrange & Act
            DateTime result = await SUT.BuildCommand()
                .ForScalar<DateTime>("SELECT [SampleDateTime] FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBe(DateTime.MinValue);

            // Print result
            WriteLine(result);
        }
    }
}
