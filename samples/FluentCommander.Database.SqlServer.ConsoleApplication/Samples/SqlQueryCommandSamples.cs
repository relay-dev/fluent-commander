﻿using ConsoleApplication.SqlServer.Framework;
using Core.Plugins.Extensions;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

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
        private void ExecuteSqlWithInput()
        {
            SqlQueryCommandResult result = _databaseCommander.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .Execute();

            Console.WriteLine("Row count: {0}", result.DataTable.Rows);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        protected override void Init()
        {
            SampleMethods = new List<SampleMethod>
            {
                new SampleMethod("1", "ExecuteSqlWithInput()", ExecuteSqlWithInput)
            };
        }
    }
}
