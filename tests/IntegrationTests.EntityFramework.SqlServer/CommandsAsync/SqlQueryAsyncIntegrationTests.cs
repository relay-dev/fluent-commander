using FluentCommander.EntityFramework;
using FluentCommander.IntegrationTests.EntityFramework.SqlServer.Entities;
using FluentCommander.SqlQuery;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.IntegrationTests.EntityFramework.SqlServer.CommandsAsync
{
    [Collection("Service Provider collection")]
    public class SqlQueryAsyncIntegrationTests : EntityFrameworkSqlServerIntegrationTest<DatabaseCommanderDomainContext>
    {
        public SqlQueryAsyncIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public async Task ExecuteSqlQueryCommandAsync_WithInputParameters_ShouldReturnDataTable()
        {
            // Arrange & Act
            SqlQueryResult result = await SUT.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }
    }
}
