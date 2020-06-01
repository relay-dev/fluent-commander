using FluentCommander.EntityFramework;
using Shouldly;
using System;
using IntegrationTests.EntityFramework.SqlServer.Entities;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.EntityFramework.SqlServer.Commands
{
    [Collection("Service Provider collection")]
    public class ScalarCommandIntegrationTests : EntityFrameworkSqlServerIntegrationTest<DatabaseCommanderDomainContext>
    {
        public ScalarCommandIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
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
