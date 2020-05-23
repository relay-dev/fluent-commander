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
    /// This sample class demonstrates how to build command for a SQL statement
    /// </notes>
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
        private async Task ExecuteSqlWithInput()
        {
            SqlQueryCommandResult result = await _databaseCommander.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Row count: {0}", result.Count);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Input parameters require a database type parameter, which can often be inferred by looking at the type of the parameter value
        /// If that default behavior does not meet your needs, you can specify the database type of your input parameter using this variation of AddInputParameter()
        /// </notes>
        private async Task ExecuteSqlWithInputSpecifyingType()
        {
            SqlQueryCommandResult result = await _databaseCommander.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1, DbType.Int32)
                .AddInputParameter("SampleVarChar", "Row 1", DbType.String)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Row count: {0}", result.Count);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        protected override void Init()
        {
            SampleMethods = new List<SampleMethodAsync>
            {
                new SampleMethodAsync("1", "ExecuteSqlWithInput()", async () => await ExecuteSqlWithInput()),
                new SampleMethodAsync("2", "ExecuteSqlWithInputSpecifyingType()", async () => await ExecuteSqlWithInputSpecifyingType())
            };
        }
    }
}
