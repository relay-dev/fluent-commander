using ConsoleApplication.SqlServer.Framework;
using Core.Plugins.Extensions;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

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
        private void ExecuteSql()
        {
            DataTable dataTable = _databaseCommander.ExecuteSql("SELECT * FROM [dbo].[SampleTable]");

            Console.WriteLine(dataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Any SQL query that returns exactly 1 row and 1 column can be executed as a scalar and cast to the return type
        /// </notes>
        private void ExecuteScalar()
        {
            string sampleVarChar = _databaseCommander.ExecuteScalar<string>("SELECT [SampleVarChar] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1");

            int sampleInt = _databaseCommander.ExecuteScalar<int>("SELECT [SampleInt] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1");

            DateTime sampleDateTime = _databaseCommander.ExecuteScalar<DateTime>("SELECT [SampleDateTime] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1");

            Guid sampleUniqueIdentifier = _databaseCommander.ExecuteScalar<Guid>("SELECT [SampleUniqueIdentifier] FROM [dbo].[SampleTable] WHERE [SampleTableID] = 1");

            Console.WriteLine($"SampleVarChar = '{sampleVarChar}'");
            Console.WriteLine($"SampleInt = '{sampleInt}'", sampleInt);
            Console.WriteLine($"SampleDateTime = '{sampleDateTime}'", sampleDateTime);
            Console.WriteLine($"SampleUniqueIdentifier = '{sampleUniqueIdentifier}'", sampleUniqueIdentifier);
        }

        /// <notes>
        /// UPDATE and DELETE statements can be issued
        /// </notes>
        private void ExecuteNonQuery()
        {
            int rowCountAffected = _databaseCommander.ExecuteNonQuery($"UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = '{Guid.NewGuid()}' WHERE [SampleTableID] = 1");

            Console.WriteLine("Row count affected: {0}", rowCountAffected);
        }

        /// <notes>
        /// Simple stored procedures can be called
        /// </notes>
        private void ExecuteStoredProcedure()
        {
            // Stored procedure with no input or output
            _databaseCommander.ExecuteStoredProcedure("[dbo].[usp_NoInput_NoOutput_NoResult]");

            // Stored procedure with no input with a DataTable as output
            DataTable dataTable = _databaseCommander.ExecuteStoredProcedure("[dbo].[usp_NoInput_NoOutput_TableResult]");

            Console.WriteLine(dataTable.ToPrintFriendly());
        }

        /// <notes>
        /// The server name can be easily obtained
        /// </notes>
        private void GetServerName()
        {
            string serverName = _databaseCommander.GetServerName();

            Console.WriteLine(serverName);
        }

        protected override void Init()
        {
            SampleMethods = new List<SampleMethod>
            {
                new SampleMethod("1", "ExecuteSql()", ExecuteSql),
                new SampleMethod("2", "ExecuteScalar()", ExecuteScalar),
                new SampleMethod("3", "ExecuteNonQuery()", ExecuteNonQuery),
                new SampleMethod("4", "ExecuteStoredProcedure()", ExecuteStoredProcedure),
                new SampleMethod("5", "GetServerName()", GetServerName)
            };
        }
    }
}
