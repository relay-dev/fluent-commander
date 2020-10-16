using Consolater;
using FluentCommander.BulkCopy;
using FluentCommander.Samples.Setup.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Samples.Commands
{
    /// <notes>
    /// The Bulk Copy function is supported if you want to insert a batch of records at once from a DataTable
    /// When Bulk Copying, SQL Server requires a mapping between source (the DataTable you want to persist) and the destination (the database on the server)
    /// </notes>
    [ConsoleAppMenuItem]
    public class BulkCopySamples : ConsoleAppBase
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
        [ConsoleAppSelection(Key = "1")]
        public async Task BulkCopyUsingAutoMapping()
        {
            DataTable dataTable = GetDataToInsert();

            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .Mapping(mapping => mapping.UseAutoMap())
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
        }

        /// <notes>
        /// This variation automatically maps between the source and destination, but also allows you to specify mappings where you know the column names do not match
        /// This works well when you want to use the auto-mapping feature, but you need to specify some additional details
        /// </notes>
        [ConsoleAppSelection(Key = "2")]
        public async Task BulkCopyUsingPartialMap()
        {
            DataTable dataTable = GetDataToInsert();

            // Alter the DataTable to simulate a source where the SampleVarChar field is named something different
            dataTable.Columns["SampleVarChar"].ColumnName = "SampleString";

            // Bulk Copy
            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .Mapping(mapping => mapping.UsePartialMap(new ColumnMapping(new List<ColumnMap>
                {
                    new ColumnMap
                    {
                        Source = "SampleString",
                        Destination = "SampleVarChar"
                    }
                })))
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
        }

        /// <notes>
        /// This variation relies on you to specify mappings where you know the column names do not match
        /// This works well when you have a significant mismatch between the column names of the source and the destination
        /// </notes>
        [ConsoleAppSelection(Key = "3")]
        public async Task BulkCopyUsingMap()
        {
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

            // Bulk Copy
            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .Mapping(mapping => mapping.UseMap(columnMapping))
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
        }

        /// <notes>
        /// When you have an entity type that reflects the shape of the table, you can use it to drive your mappings
        /// </notes>
        [ConsoleAppSelection(Key = "4")]
        public async Task BulkCopyUsingStronglyTypedMap()
        {
            DataTable dataTable = GetDataToInsert();

            // Alter the DataTable to simulate a source where all column names are different than the destination
            dataTable.Columns["SampleInt"].ColumnName = "Column1";
            dataTable.Columns["SampleSmallInt"].ColumnName = "Column2";
            dataTable.Columns["SampleTinyInt"].ColumnName = "Column3";
            dataTable.Columns["SampleBit"].ColumnName = "Column4";
            dataTable.Columns["SampleDecimal"].ColumnName = "Column5";
            dataTable.Columns["SampleFloat"].ColumnName = "Column6";
            dataTable.Columns["SampleVarChar"].ColumnName = "Column7";

            // Bulk Copy
            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy<SampleEntity>()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .Mapping(mapping => mapping.UseMap(entity =>
                {
                    entity.Property(e => e.SampleInt).MapFrom("Column1");
                    entity.Property(e => e.SampleSmallInt).MapFrom("Column2");
                    entity.Property(e => e.SampleTinyInt).MapFrom("Column3");
                    entity.Property(e => e.SampleBit).MapFrom("Column4");
                    entity.Property(e => e.SampleDecimal).MapFrom("Column5");
                    entity.Property(e => e.SampleFloat).MapFrom("Column6");
                    entity.Property(e => e.SampleVarChar).MapFrom("Column7");
                }))
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
        }

        /// <notes>
        /// All BulkCopy options are exposed via the Options API
        /// </notes>
        [ConsoleAppSelection(Key = "5")]
        public async Task BulkCopyUsingOptions()
        {
            DataTable dataTable = GetDataToInsert();

            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .Mapping(mapping => mapping.UseAutoMap())
                .Options(options => options.KeepNulls().CheckConstraints().TableLock(false))
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
        }

        /// <notes>
        /// BulkCopy order hints are exposed via the OrderHints API
        /// </notes>
        [ConsoleAppSelection(Key = "6")]
        public async Task BulkCopyUsingOrderHints()
        {
            DataTable dataTable = GetDataToInsert();

            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .Mapping(mapping => mapping.UseAutoMap())
                .OrderHints(hints => hints.OrderBy("SampleInt").OrderByDescending("SampleSmallInt"))
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
        }

        /// <notes>
        /// The OnRowsCopied event can be subscribed to
        /// </notes>
        [ConsoleAppSelection(Key = "7")]
        public async Task BulkCopyUsingEvents()
        {
            DataTable dataTable = GetDataToInsert();

            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy()
                .From(dataTable)
                .Into("[dbo].[SampleTable]")
                .Mapping(mapping => mapping.UseAutoMap())
                .Events(events => events.NotifyAfter(10).OnRowsCopied((sender, e) =>
                {
                    var sqlRowsCopiedEventArgs = (SqlRowsCopiedEventArgs)e;

                    Console.WriteLine($"Total rows copied: {sqlRowsCopiedEventArgs.RowsCopied}");
                }))
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
        }
        
        /// <notes>
        /// All APIs
        /// </notes>
        [ConsoleAppSelection(Key = "8")]
        public async Task BulkCopyUsingAllApis()
        {
            DataTable dataTable = GetDataToInsert();

            // Alter the DataTable to simulate a source where the SampleVarChar field is named something different
            dataTable.Columns["SampleVarChar"].ColumnName = "SampleString";

            BulkCopyResult result = await _databaseCommander.BuildCommand()
                .ForBulkCopy<SampleEntity>()
                .From(dataTable, DataRowState.Added)
                .Into("[dbo].[SampleTable]")
                .BatchSize(100)
                .Timeout(TimeSpan.FromSeconds(30))
                .Options(options => options.KeepNulls().CheckConstraints().TableLock(false).OpenConnectionWithoutRetry())
                .Mapping(mapping => mapping.UsePartialMap(entity =>
                {
                    entity.Property(e => e.SampleVarChar).MapFrom("SampleString");
                }))
                .OrderHints(hints => hints.Build(entity =>
                {
                    entity.Property(e => e.SampleInt).OrderByDescending();
                }))
                .Events(events => events.NotifyAfter(10).OnRowsCopied((sender, e) =>
                {
                    var sqlRowsCopiedEventArgs = (SqlRowsCopiedEventArgs)e;

                    Console.WriteLine($"Total rows copied: {sqlRowsCopiedEventArgs.RowsCopied}");
                }))
                .ExecuteAsync(new CancellationToken());

            int rowCountCopied = result.RowCountCopied;

            Console.WriteLine("Row count copied: {0}", rowCountCopied);
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
                dataRow["CreatedBy"] = this.GetType().Assembly.FullName;
                dataRow["CreatedDate"] = DateTime.UtcNow;

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
