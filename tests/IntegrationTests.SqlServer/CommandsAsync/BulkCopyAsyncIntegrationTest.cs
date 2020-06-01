using FluentCommander;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentCommander.BulkCopy;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.SqlServer.CommandsAsync
{
    [Collection("Service Provider collection")]
    public class BulkCopyAsyncIntegrationTest : SqlServerIntegrationTest<IDatabaseCommander>
    {
        private const int RowCount = 100;

        public BulkCopyAsyncIntegrationTest(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public async Task ExecuteBulkCopyAsync_ShouldCreateNewRows_WhenUsingAutoMapper()
        {
            // Arrange
            int expectedCount = ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]") + RowCount;
            DataTable dataTable = GetDataToInsert();

            // Act
            BulkCopyResult result = await SUT.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .MappingOptions(opt => opt.UseAutoMap())
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]").ShouldBe(expectedCount);

            // Print
            WriteLine(expectedCount);
        }

        [Fact]
        public async Task ExecuteBulkCopyAsync_ShouldCreateNewRows_WhenUsingPartialMapper()
        {
            // Arrange
            int expectedCount = ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]") + RowCount;
            DataTable dataTable = GetDataToInsert();

            // Alter the DataTable to simulate a source where the SampleVarChar field is named something different
            dataTable.Columns["SampleVarChar"].ColumnName = "SampleString";

            // Now specify the mapping
            var columnMapping = new ColumnMapping
            {
                ColumnMaps = new List<ColumnMap>
                {
                    new ColumnMap("SampleString", "SampleVarChar")
                }
            };

            // Act
            BulkCopyResult result = await SUT.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .MappingOptions(opt => opt.UsePartialMap(columnMapping))
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]").ShouldBe(expectedCount);

            // Print
            WriteLine(expectedCount);
        }

        [Fact]
        public async Task ExecuteBulkCopyAsync_ShouldCreateNewRows_WhenUsingMap()
        {
            // Arrange
            int expectedCount = ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]") + RowCount;
            DataTable dataTable = GetDataToInsert();

            // Alter the DataTable to simulate a source where all column names are different than the destination
            dataTable.Columns["SampleInt"].ColumnName = "Column1";
            dataTable.Columns["SampleSmallInt"].ColumnName = "Column2";
            dataTable.Columns["SampleTinyInt"].ColumnName = "Column3";
            dataTable.Columns["SampleBit"].ColumnName = "Column4";
            dataTable.Columns["SampleDecimal"].ColumnName = "Column5";
            dataTable.Columns["SampleFloat"].ColumnName = "Column6";
            dataTable.Columns["SampleVarChar"].ColumnName = "Column7";

            // Now specify the mapping
            var columnMapping = new ColumnMapping
            {
                ColumnMaps = new List<ColumnMap>
                {
                    new ColumnMap("Column1", "SampleInt"),
                    new ColumnMap("Column2", "SampleSmallInt"),
                    new ColumnMap("Column3", "SampleTinyInt"),
                    new ColumnMap("Column4", "SampleBit"),
                    new ColumnMap("Column5", "SampleDecimal"),
                    new ColumnMap("Column6", "SampleFloat"),
                    new ColumnMap("Column7", "SampleVarChar"),
                }
            };

            // Act
            BulkCopyResult result = await SUT.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .MappingOptions(opt => opt.UseMap(columnMapping))
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]").ShouldBe(expectedCount);

            // Print
            WriteLine(expectedCount);
        }

        private DataTable GetDataToInsert()
        {
            DataTable dataTable = ExecuteSql("SELECT * FROM [dbo].[SampleTable] WHERE 1 = 0");

            for (int i = 0; i < RowCount; i++)
            {
                DataRow dataRow = dataTable.NewRow();

                dataRow["SampleInt"] = 1;
                dataRow["SampleSmallInt"] = 1;
                dataRow["SampleTinyInt"] = 1;
                dataRow["SampleBit"] = true;
                dataRow["SampleDecimal"] = 1;
                dataRow["SampleFloat"] = 1;
                dataRow["SampleDateTime"] = DateTime.UtcNow;
                dataRow["SampleUniqueIdentifier"] = Guid.NewGuid();
                dataRow["SampleVarChar"] = $"Row {i + 4}";
                dataRow["CreatedBy"] = TestUsername;
                dataRow["CreatedDate"] = Timestamp;

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
