using Core.Plugins.Extensions;
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
    /// This sample class demonstrates how to build command for a SQL statement
    /// </notes>
    [SampleFixture]
    public class SqlQueryCommandSamples : CommandSampleBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public SqlQueryCommandSamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// SQL queries with parameters can be parameterized for SQL Server to cache the execution plan and to avoid injection
        /// This method demonstrates how to query the database with inline SQL using input parameters
        /// </notes>
        [Sample(Key = "1")]
        public async Task ExecuteSqlWithInput()
        {
            SqlQueryResult result = await _databaseCommander.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", dataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Input parameters require a database type parameter, which can often be inferred by looking at the type of the parameter value
        /// If that default behavior does not meet your needs, you can specify the database type of your input parameter using this variation of AddInputParameter()
        /// </notes>
        [Sample(Key = "2")]
        public async Task ExecuteSqlWithInputSpecifyingType()
        {
            SqlQueryResult result = await _databaseCommander.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1, DbType.Int32)
                .AddInputParameter("SampleVarChar", "Row 1", DbType.String)
                .Timeout(TimeSpan.FromSeconds(30))
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", dataTable.ToPrintFriendly());
        }
    }
}
