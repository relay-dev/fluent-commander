using Consolater;
using FluentCommander.StoredProcedure;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Samples.Commands
{
    /// <notes>
    /// Basic commands exist directly on the IDatabaseCommander so that you're not required to build a full command if you don't need one
    /// More complex commands that require parameters and return values should not use this strategy
    /// </notes>
    [ConsoleAppMenuItem]
    public class ParameterlessSamples : Sample
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly IDatabaseRequestHandler _databaseRequestHandler;

        public ParameterlessSamples(
            IDatabaseCommander databaseCommander,
            IDatabaseRequestHandler databaseRequestHandler,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
            _databaseRequestHandler = databaseRequestHandler;
        }

        /// <notes>
        /// Any SQL query can be executed with the result being returned as a DataTable
        /// </notes>
        [ConsoleAppSelection(Key = "1")]
        public async Task ExecuteSql()
        {
            DataTable dataTable = await _databaseRequestHandler
                .ExecuteSqlAsync("SELECT * FROM [dbo].[SampleTable]", new CancellationToken());

            Console.WriteLine(Print(dataTable));
        }

        /// <notes>
        /// Any SQL query that returns exactly 1 row and 1 column can be executed as a scalar and cast to the return type
        /// </notes>
        [ConsoleAppSelection(Key = "2")]
        public async Task ExecuteScalar()
        {
            string sampleVarChar = await _databaseRequestHandler
                .ExecuteScalarAsync<string>("SELECT [SampleVarChar] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1", new CancellationToken());

            int sampleInt = await _databaseRequestHandler
                .ExecuteScalarAsync<int>("SELECT [SampleInt] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1", new CancellationToken());

            DateTime sampleDateTime = await _databaseRequestHandler
                .ExecuteScalarAsync<DateTime>("SELECT [SampleDateTime] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1", new CancellationToken());

            Guid sampleUniqueIdentifier = await _databaseRequestHandler
                .ExecuteScalarAsync<Guid>("SELECT [SampleUniqueIdentifier] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1", new CancellationToken());

            Console.WriteLine($"SampleVarChar = '{sampleVarChar}'");
            Console.WriteLine($"SampleInt = '{sampleInt}'", sampleInt);
            Console.WriteLine($"SampleDateTime = '{sampleDateTime}'", sampleDateTime);
            Console.WriteLine($"SampleUniqueIdentifier = '{sampleUniqueIdentifier}'", sampleUniqueIdentifier);
        }

        /// <notes>
        /// UPDATE and DELETE statements can be issued
        /// </notes>
        [ConsoleAppSelection(Key = "3")]
        public async Task ExecuteNonQuery()
        {
            int rowCountAffected = await _databaseRequestHandler
                .ExecuteNonQueryAsync($"UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = '{Guid.NewGuid()}' WHERE [SampleTableID] = 1", new CancellationToken());

            Console.WriteLine("Row count affected: {0}", rowCountAffected);
        }

        /// <notes>
        /// Simple stored procedures can be called
        /// </notes>
        [ConsoleAppSelection(Key = "4")]
        public async Task ExecuteStoredProcedure()
        {
            // Stored procedure with no input or output
            await _databaseRequestHandler
                .ExecuteStoredProcedureAsync(new StoredProcedureRequest("[dbo].[usp_NoInput_NoOutput_NoResult]"), new CancellationToken());

            // Stored procedure with no input with a DataTable as output
            StoredProcedureResult result = await _databaseRequestHandler
                .ExecuteStoredProcedureAsync(new StoredProcedureRequest("[dbo].[usp_NoInput_NoOutput_TableResult]"), new CancellationToken());

            Console.WriteLine(Print(result.DataTable));
        }

        /// <notes>
        /// The server name can be easily obtained
        /// </notes>
        [ConsoleAppSelection(Key = "5")]
        public async Task GetServerName()
        {
            string serverName = await _databaseCommander.GetServerNameAsync(new CancellationToken());

            Console.WriteLine(serverName);
        }
    }
}
