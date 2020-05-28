using FluentCommander;
using FluentCommander.EntityFramework;
using IntegrationTests.EntityFramework.Entities;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.EntityFramework.Commands
{
    [Collection("Service Provider collection")]
    public class SqlQueryCommandIntegrationTests : EntityFrameworkIntegrationTest<DatabaseCommanderDomainContext>
    {
        public SqlQueryCommandIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public void ExecuteSqlQueryCommand_WithInputParameters_ShouldReturnDataTable()
        {
            // Arrange & Act
            SqlQueryResult result = SUT.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }
    }
}
