using FluentCommander;
using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentCommander.BulkCopy;

namespace Samples.Commands
{
    /// <notes>
    /// The Bulk Copy function is supported if you want to insert a batch of records at once from a DataTable
    /// When Bulk Copying, SQL Server requires a mapping between source (the DataTable you want to persist) and the destination (the database on the server)
    /// </notes>
    [SampleFixture]
    public class BulkCopySamples : SampleBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public BulkCopySamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// This variation automatically maps between the source and destination. The details of this implementation can be found in the FluentCommander.Utility.AutoMapper class
        /// This works well in circumstances where you control the source and can easily ensure the DataTable column names match the column names on the database table
        /// </notes>
        [Sample(Key = "1")]
        public async Task BulkCopyUsingAutoMapping()
        {
            DataTable dataTable = GetDataToInsert();
            int countBefore = ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]");

            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .MappingOptions(opt => opt.UseAutoMap())
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
            Console.WriteLine("Count before: {0}", countBefore);
            Console.WriteLine("Count after: {0}", ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]"));
        }

        /// <notes>
        /// This variation automatically maps between the source and destination, but also allows you to specify mappings where you know the column names do not match
        /// This works well when you want to use the auto-mapping feature, but you need to specify some additional details
        /// </notes>
        [Sample(Key = "2")]
        public async Task BulkCopyUsingPartialMap()
        {
            DataTable dataTable = GetDataToInsert();
            int countBefore = ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]");

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

            // Bulk Copy
            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .MappingOptions(opt => opt.UsePartialMap(columnMapping))
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
            Console.WriteLine("Count before: {0}", countBefore);
            Console.WriteLine("Count after: {0}", ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]"));
        }

        /// <notes>
        /// This variation relies on you to specify mappings where you know the column names do not match
        /// This works well when you have a significant mismatch between the column names of the source and the destination
        /// </notes>
        [Sample(Key = "3")]
        public async Task BulkCopyUsingMap()
        {
            DataTable dataTable = GetDataToInsert();
            int countBefore = ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]");

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

            // Bulk Copy
            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .MappingOptions(opt => opt.UseMap(columnMapping))
                .Timeout(TimeSpan.FromSeconds(30))
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
            Console.WriteLine("Count before: {0}", countBefore);
            Console.WriteLine("Count after: {0}", ExecuteScalar<int>("SELECT COUNT(1) FROM [dbo].[SampleTable]"));
        }

        private DataTable GetDataToInsert(int rowCount = 100)
        {
            DataTable dataTable = ExecuteSql("SELECT * FROM [dbo].[SampleTable] WHERE 1 = 0");

            for (int i = 0; i < rowCount; i++)
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
                dataRow["CreatedBy"] = GetType().Assembly.FullName;
                dataRow["CreatedDate"] = DateTime.UtcNow;

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
