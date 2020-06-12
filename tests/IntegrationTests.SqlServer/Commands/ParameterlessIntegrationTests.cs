using FluentCommander;
using Shouldly;
using System;
using System.Data;
using FluentCommander.StoredProcedure;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.SqlServer.Commands
{
    [Collection("Service Provider collection")]
    public class ParameterlessIntegrationTests : SqlServerIntegrationTest<IDatabaseCommander>
    {
        public ParameterlessIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public void ExecuteSql_ShouldReturnDataTable_WithNoParameters()
        {
            // Arrange
            string sql = "SELECT * FROM [dbo].[SampleTable]";

            // Act
            DataTable dataTable = SUT.ExecuteSql(sql);

            // Assert
            dataTable.ShouldNotBeNull();
            dataTable.Rows.Count.ShouldBeGreaterThan(0);

            // Print
            WriteLine(dataTable);
        }

        [Fact]
        public void ExecuteScalar_ShouldReturnResult_WithNoParameters()
        {
            // Arrange
            string sql = "SELECT [SampleVarChar] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1";

            // Act
            string sampleVarChar = SUT.ExecuteScalar<string>(sql);

            // Assert
            sampleVarChar.ShouldNotBeNullOrEmpty();

            // Print
            WriteLine(sampleVarChar);
        }

        [Fact]
        public void ExecuteNonQuery_ShouldUpdateAsExpected_WithNoParameters()
        {
            // Arrange
            Guid newGuid = Guid.NewGuid();
            string sql = $"UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = '{newGuid}' WHERE [SampleTableID] = 1";

            // Act
            int rowCountAffected = SUT.ExecuteNonQuery(sql);

            // Assert
            rowCountAffected.ShouldBe(1);
            GetUpdatedGuid().ShouldBe(newGuid);

            // Print
            WriteLine(rowCountAffected);
        }

        [Fact]
        public void ExecuteStoredProcedure_ShouldReturnDataTable_WithNoParameters()
        {
            // Arrange
            var storedProcedureRequest = new StoredProcedureRequest("[dbo].[usp_NoInput_NoOutput_TableResult]");

            // Act
            StoredProcedureResult result = SUT.ExecuteStoredProcedure(storedProcedureRequest);

            // Assert
            result.DataTable.ShouldNotBeNull();
            result.DataTable.Rows.Count.ShouldBeGreaterThan(0);

            // Print
            WriteLine(result.DataTable);
        }

        [Fact]
        public void GetServerName_ShouldReturnServerName_WhenConnectedCorrectly()
        {
            // Arrange & Act
            string serverName = SUT.GetServerName();

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
