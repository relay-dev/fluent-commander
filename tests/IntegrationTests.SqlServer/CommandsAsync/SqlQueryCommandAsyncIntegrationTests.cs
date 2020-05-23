using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Database;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.SqlServer.CommandsAsync
{
    [Collection("Service Provider collection")]
    public class SqlQueryCommandAsyncIntegrationTests : IntegrationTest<IDatabaseCommander>
    {
        public SqlQueryCommandAsyncIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public async Task ExecuteSqlQueryCommandAsync_WithInputParameters_ShouldReturnDataTable()
        {
            // Arrange & Act
            SqlQueryCommandResult result = await SUT.BuildCommand()
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
