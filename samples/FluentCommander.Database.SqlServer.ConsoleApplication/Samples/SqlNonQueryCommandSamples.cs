using ConsoleApplication.SqlServer.Framework;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ConsoleApplication.SqlServer.Samples
{
    /// <notes>
    /// This sample class demonstrates how to build command for a SQL non-query, such as an INSERT, UPDATE or DELETE
    /// </notes>
    public class SqlNonQueryCommandSamples : CommandSampleBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public SqlNonQueryCommandSamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// SQL update statements with parameters can be parameterized for SQL Server to cache the execution plan and to avoid injection
        /// </notes>
        private void ExecuteParameterizedUpdateSql()
        {
            Guid newGuid = Guid.NewGuid();
            string modifiedBy = "FluentCommander";
            DateTime modifiedDate = DateTime.UtcNow;

            SqlNonQueryCommandResult result = _databaseCommander.BuildCommand()
                .ForSqlNonQuery("UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = @NewGuid, [ModifiedBy] = @ModifiedBy, [ModifiedDate] = @ModifiedDate WHERE [SampleTableID] = @SampleTableID")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("NewGuid", newGuid)
                .AddInputParameter("ModifiedBy", modifiedBy)
                .AddInputParameter("ModifiedDate", modifiedDate)
                .Execute();

            Console.WriteLine("Row count affected: {0}", result.RowCountAffected);
        }

        /// <notes>
        /// SQL insert and delete statements with parameters can be parameterized for SQL Server to cache the execution plan and to avoid injection
        /// </notes>
        private void ExecuteParameterizedInsertDeleteSql()
        {
            string sampleVarChar = "Temporary Row";
            string createdBy = "FluentCommander";
            DateTime createdDate = DateTime.UtcNow;

            string insertSql =
@"INSERT INTO [dbo].[SampleTable]
           ([SampleInt]
           ,[SampleSmallInt]
           ,[SampleTinyInt]
           ,[SampleBit]
           ,[SampleDecimal]
           ,[SampleFloat]
           ,[SampleVarChar]
           ,[CreatedBy]
           ,[CreatedDate])
     VALUES
           (1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,@SampleVarChar
           ,@CreatedBy
           ,@CreatedDate)";

            SqlNonQueryCommandResult insertResult = _databaseCommander.BuildCommand()
                .ForSqlNonQuery(insertSql)
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .AddInputParameter("CreatedBy", createdBy)
                .AddInputParameter("CreatedDate", createdDate)
                .Execute();

            SqlNonQueryCommandResult deleteResult = _databaseCommander.BuildCommand()
                .ForSqlNonQuery("DELETE FROM [dbo].[SampleTable] WHERE [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .Execute();

            Console.WriteLine("Row count affected: {0}", insertResult.RowCountAffected);
            Console.WriteLine("Row count affected: {0}", deleteResult.RowCountAffected);
        }

        protected override void Init()
        {
            SampleMethods = new List<SampleMethod>
            {
                new SampleMethod("1", "ExecuteParameterizedUpdateSql()", ExecuteParameterizedUpdateSql),
                new SampleMethod("2", "ExecuteParameterizedInsertDeleteSql()", ExecuteParameterizedInsertDeleteSql)
            };
        }
    }
}
