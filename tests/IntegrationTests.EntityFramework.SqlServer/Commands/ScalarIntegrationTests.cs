using FluentCommander.EntityFramework;
using IntegrationTests.EntityFramework.SqlServer.Entities;
using Shouldly;
using System;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.EntityFramework.SqlServer.Commands
{
    [Collection("Service Provider collection")]
    public class ScalarIntegrationTests : EntityFrameworkSqlServerIntegrationTest<DatabaseCommanderDomainContext>
    {
        public ScalarIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public void ExecuteScalarCommand_WithInputParameters_ShouldReturnResult()
        {
            // Arrange & Act
            DateTime result = SUT.BuildCommand()
                .ForScalar<DateTime>("SELECT [SampleDateTime] FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .Execute();

            // Assert
            result.ShouldNotBe(DateTime.MinValue);

            // Print result
            WriteLine(result);
        }
    }
}
