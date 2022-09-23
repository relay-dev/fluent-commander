using FluentCommander.StoredProcedure;
using Shouldly;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.IntegrationTests.SqlServer.CommandsAsync
{
    [Collection("Service Provider collection")]
    public class ParameterlessAsyncIntegrationTests : SqlServerIntegrationTest<IDatabaseRequestHandler>
    {
        public ParameterlessAsyncIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public async Task ExecuteSqlAsync_ShouldReturnDataTable_WithNoParameters()
        {
            // Arrange
            string sql = "SELECT * FROM [dbo].[SampleTable]";

            // Act
            DataTable dataTable = await SUT.ExecuteSqlAsync(sql, new CancellationToken());

            // Assert
            dataTable.ShouldNotBeNull();
            dataTable.Rows.Count.ShouldBeGreaterThan(0);

            // Print
            WriteLine(dataTable);
        }

        [Fact]
        public async Task ExecuteScalarAsync_ShouldReturnResult_WithNoParameters()
        {
            // Arrange
            string sql = "SELECT [SampleVarChar] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1";

            // Act
            string sampleVarChar = await SUT.ExecuteScalarAsync<string>(sql, new CancellationToken());

            // Assert
            sampleVarChar.ShouldNotBeNullOrEmpty();

            // Print
            WriteLine(sampleVarChar);
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_ShouldUpdateAsExpected_WithNoParameters()
        {
            // Arrange
            Guid newGuid = Guid.NewGuid();
            string sql = $"UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = '{newGuid}' WHERE [SampleTableID] = 1";

            // Act
            int rowCountAffected = await SUT.ExecuteNonQueryAsync(sql, new CancellationToken());

            // Assert
            rowCountAffected.ShouldBe(1);
            GetUpdatedGuid().ShouldBe(newGuid);

            // Print
            WriteLine(rowCountAffected);
        }

        [Fact]
        public async Task ExecuteStoredProcedureAsync_ShouldReturnDataTable_WithNoParameters()
        {
            // Arrange
            var storedProcedureRequest = new StoredProcedureRequest("[dbo].[usp_NoInput_NoOutput_TableResult]");

            // Act
            StoredProcedureResult result = await SUT.ExecuteStoredProcedureAsync(storedProcedureRequest, new CancellationToken());

            // Assert
            result.DataTable.ShouldNotBeNull();
            result.DataTable.Rows.Count.ShouldBeGreaterThan(0);

            // Print
            WriteLine(result.DataTable);
        }

        [Fact]
        public async Task GetServerNameAsync_ShouldReturnServerName_WhenConnectedCorrectly()
        {
            // Arrange & Act
            string serverName = await SUT.GetServerNameAsync(new CancellationToken());

            // Assert
            serverName.ShouldNotBeNullOrEmpty();

            // Print
            WriteLine(serverName);
        }

        private Guid GetUpdatedGuid()
        {
            DataTable dataTable = ExecuteSql("SELECT [SampleUniqueIdentifier] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1");

            if (dataTable.Rows.Count != 1 || dataTable.Columns.Count != 1)
                return Guid.Empty;

            string s = dataTable.Rows[0][0].ToString();

            if (!Guid.TryParse(s, out Guid guidToReturn))
            {
                return Guid.Empty;
            }

            return guidToReturn;
        }
    }
}
