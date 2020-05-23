using ConsoleApplication.SqlServer.Framework;
using Core.Plugins.Extensions;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Samples
{
    /// <notes>
    /// Basic commands exist directly on the IDatabaseCommander so that you're not required to build a full command if you don't need one
    /// More complex commands that require parameters and return values should not use this strategy
    /// </notes>
    public class BasicCommandSamples : CommandSampleBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public BasicCommandSamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// Any SQL query can be executed with the result being returned as a DataTable
        /// </notes>
        private async Task ExecuteSql()
        {
            DataTable dataTable = await _databaseCommander
                .ExecuteSqlAsync("SELECT * FROM [dbo].[SampleTable]", new CancellationToken());

            Console.WriteLine(dataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Any SQL query that returns exactly 1 row and 1 column can be executed as a scalar and cast to the return type
        /// </notes>
        private async Task ExecuteScalar()
        {
            string sampleVarChar = await _databaseCommander
                .ExecuteScalarAsync<string>("SELECT [SampleVarChar] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1", new CancellationToken());

            int sampleInt = await _databaseCommander
                .ExecuteScalarAsync<int>("SELECT [SampleInt] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1", new CancellationToken());

            DateTime sampleDateTime = await _databaseCommander
                .ExecuteScalarAsync<DateTime>("SELECT [SampleDateTime] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1", new CancellationToken());

            Guid sampleUniqueIdentifier = await _databaseCommander
                .ExecuteScalarAsync<Guid>("SELECT [SampleUniqueIdentifier] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1", new CancellationToken());

            Console.WriteLine($"SampleVarChar = '{sampleVarChar}'");
            Console.WriteLine($"SampleInt = '{sampleInt}'", sampleInt);
            Console.WriteLine($"SampleDateTime = '{sampleDateTime}'", sampleDateTime);
            Console.WriteLine($"SampleUniqueIdentifier = '{sampleUniqueIdentifier}'", sampleUniqueIdentifier);
        }

        /// <notes>
        /// UPDATE and DELETE statements can be issued
        /// </notes>
        private async Task ExecuteNonQuery()
        {
            int rowCountAffected = await _databaseCommander
                .ExecuteNonQueryAsync($"UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = '{Guid.NewGuid()}' WHERE [SampleTableID] = 1", new CancellationToken());

            Console.WriteLine("Row count affected: {0}", rowCountAffected);
        }

        /// <notes>
        /// Simple stored procedures can be called
        /// </notes>
        private async Task ExecuteStoredProcedure()
        {
            // Stored procedure with no input or output
            await _databaseCommander
                .ExecuteStoredProcedureAsync("[dbo].[usp_NoInput_NoOutput_NoResult]", new CancellationToken());

            // Stored procedure with no input with a DataTable as output
            StoredProcedureResult result = await _databaseCommander
                .ExecuteStoredProcedureAsync("[dbo].[usp_NoInput_NoOutput_TableResult]", new CancellationToken());

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// The server name can be easily obtained
        /// </notes>
        private async Task GetServerName()
        {
            string serverName = await _databaseCommander.GetServerNameAsync(new CancellationToken());

            Console.WriteLine(serverName);
        }

        protected override void Init()
        {
            SampleMethods = new List<SampleMethodAsync>
            {
                new SampleMethodAsync("1", "ExecuteSql()", async () => await ExecuteSql()),
                new SampleMethodAsync("2", "ExecuteScalar()", async () => await ExecuteScalar()),
                new SampleMethodAsync("3", "ExecuteNonQuery()", async () => await ExecuteNonQuery()),
                new SampleMethodAsync("4", "ExecuteStoredProcedure()", async () => await ExecuteStoredProcedure()),
                new SampleMethodAsync("5", "GetServerName()", async () => await GetServerName())
            };
        }
    }
}
