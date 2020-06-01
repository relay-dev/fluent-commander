using FluentCommander;
using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Commands
{
    /// <notes>
    /// Basic commands exist directly on the IDatabaseCommander so that you're not required to build a full command if you don't need one
    /// More complex commands that require parameters and return values should not use this strategy
    /// </notes>
    [SampleFixture]
    public class ParameterlessCommandSamples : CommandSampleBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public ParameterlessCommandSamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// Any SQL query can be executed with the result being returned as a DataTable
        /// </notes>
        [Sample(Key = "1")]
        public async Task ExecuteSql()
        {
            DataTable dataTable = await _databaseCommander
                .ExecuteSqlAsync("SELECT * FROM [dbo].[SampleTable]", new CancellationToken());

            Console.WriteLine(Print(dataTable));
        }

        /// <notes>
        /// Any SQL query that returns exactly 1 row and 1 column can be executed as a scalar and cast to the return type
        /// </notes>
        [Sample(Key = "2")]
        public async Task ExecuteScalar()
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
        [Sample(Key = "3")]
        public async Task ExecuteNonQuery()
        {
            int rowCountAffected = await _databaseCommander
                .ExecuteNonQueryAsync($"UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = '{Guid.NewGuid()}' WHERE [SampleTableID] = 1", new CancellationToken());

            Console.WriteLine("Row count affected: {0}", rowCountAffected);
        }

        /// <notes>
        /// Simple stored procedures can be called
        /// </notes>
        [Sample(Key = "4")]
        public async Task ExecuteStoredProcedure()
        {
            // Stored procedure with no input or output
            await _databaseCommander
                .ExecuteStoredProcedureAsync(new StoredProcedureRequest("[dbo].[usp_NoInput_NoOutput_NoResult]"), new CancellationToken());

            // Stored procedure with no input with a DataTable as output
            StoredProcedureResult result = await _databaseCommander
                .ExecuteStoredProcedureAsync(new StoredProcedureRequest("[dbo].[usp_NoInput_NoOutput_TableResult]"), new CancellationToken());

            Console.WriteLine(Print(result.DataTable));
        }

        /// <notes>
        /// The server name can be easily obtained
        /// </notes>
        [Sample(Key = "5")]
        public async Task GetServerName()
        {
            string serverName = await _databaseCommander.GetServerNameAsync(new CancellationToken());

            Console.WriteLine(serverName);
        }
    }
}
