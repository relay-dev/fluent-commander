﻿using Consolater;
using FluentCommander.SqlNonQuery;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Samples.Commands
{
    /// <notes>
    /// This sample class demonstrates how to build command for a SQL non-query, such as an INSERT, UPDATE or DELETE
    /// </notes>
    [ConsoleAppMenuItem]
    public class SqlNonQuerySamples : Sample
    {
        private readonly IDatabaseCommander _databaseCommander;

        public SqlNonQuerySamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// SQL Update statements can also be parameterized for the database to cache the execution plan and prevent against injection
        /// </notes>
        [ConsoleAppSelection(Key = "1")]
        public async Task ExecuteParameterizedUpdateSql()
        {
            Guid newGuid = Guid.NewGuid();
            string modifiedBy = "FluentCommander";
            DateTime modifiedDate = DateTime.UtcNow;

            SqlNonQueryResult result = await _databaseCommander.BuildCommand()
                .ForSqlNonQuery("UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = @NewGuid, [ModifiedBy] = @ModifiedBy, [ModifiedDate] = @ModifiedDate WHERE [SampleTableID] = @SampleTableID")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("NewGuid", newGuid)
                .AddInputParameter("ModifiedBy", modifiedBy)
                .AddInputParameter("ModifiedDate", modifiedDate)
                .ExecuteAsync(new CancellationTokenSource().Token);

            int rowCountAffected = result.RowCountAffected;

            Console.WriteLine("Row count affected: {0}", rowCountAffected);
        }

        /// <notes>
        /// SQL Insert and Delete statements can also be parameterized for the database to cache the execution plan and prevent against injection
        /// </notes>
        [ConsoleAppSelection(Key = "2")]
        public async Task ExecuteParameterizedInsertDeleteSql()
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

            SqlNonQueryResult insertResult = await _databaseCommander.BuildCommand()
                .ForSqlNonQuery(insertSql)
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .AddInputParameter("CreatedBy", createdBy)
                .AddInputParameter("CreatedDate", createdDate)
                .ExecuteAsync(new CancellationTokenSource().Token);

            SqlNonQueryResult deleteResult = await _databaseCommander.BuildCommand()
                .ForSqlNonQuery("DELETE FROM [dbo].[SampleTable] WHERE [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .ExecuteAsync(new CancellationTokenSource().Token);

            int rowCountAffectedFromInsert = insertResult.RowCountAffected;
            int rowCountAffectedFromDelete = deleteResult.RowCountAffected;

            Console.WriteLine("Row count affected from Insert: {0}", rowCountAffectedFromInsert);
            Console.WriteLine("Row count affected from Delete: {0}", rowCountAffectedFromDelete);
        }
    }
}
