using ConsoleApplication.SqlServer.Framework;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        private async Task ExecuteParameterizedUpdateSql()
        {
            Guid newGuid = Guid.NewGuid();
            string modifiedBy = "FluentCommander";
            DateTime modifiedDate = DateTime.UtcNow;

            SqlNonQueryCommandResult result = await _databaseCommander.BuildCommand()
                .ForSqlNonQuery("UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = @NewGuid, [ModifiedBy] = @ModifiedBy, [ModifiedDate] = @ModifiedDate WHERE [SampleTableID] = @SampleTableID")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("NewGuid", newGuid)
                .AddInputParameter("ModifiedBy", modifiedBy)
                .AddInputParameter("ModifiedDate", modifiedDate)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Row count affected: {0}", result.RowCountAffected);
        }

        /// <notes>
        /// SQL insert and delete statements with parameters can be parameterized for SQL Server to cache the execution plan and to avoid injection
        /// </notes>
        private async Task ExecuteParameterizedInsertDeleteSql()
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

            SqlNonQueryCommandResult insertResult = await _databaseCommander.BuildCommand()
                .ForSqlNonQuery(insertSql)
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .AddInputParameter("CreatedBy", createdBy)
                .AddInputParameter("CreatedDate", createdDate)
                .ExecuteAsync(new CancellationToken());

            SqlNonQueryCommandResult deleteResult = await _databaseCommander.BuildCommand()
                .ForSqlNonQuery("DELETE FROM [dbo].[SampleTable] WHERE [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Row count affected: {0}", insertResult.RowCountAffected);
            Console.WriteLine("Row count affected: {0}", deleteResult.RowCountAffected);
        }

        protected override void Init()
        {
            SampleMethods = new List<SampleMethodAsync>
            {
                new SampleMethodAsync("1", "ExecuteParameterizedUpdateSql()", async () => await ExecuteParameterizedUpdateSql()),
                new SampleMethodAsync("2", "ExecuteParameterizedInsertDeleteSql()", async () => await ExecuteParameterizedInsertDeleteSql())
            };
        }
    }
}
