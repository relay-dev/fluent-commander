using FluentCommander;
using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Commands
{
    /// <notes>
    /// This sample class demonstrates how to build command for a SQL non-query, such as an INSERT, UPDATE or DELETE
    /// </notes>
    [SampleFixture]
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
        [Sample(Key = "1")]
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
                .ExecuteAsync(new CancellationToken());

            int rowCountAffected = result.RowCountAffected;

            Console.WriteLine("Row count affected: {0}", rowCountAffected);
        }

        /// <notes>
        /// SQL insert and delete statements with parameters can be parameterized for SQL Server to cache the execution plan and to avoid SQL injection
        /// </notes>
        [Sample(Key = "2")]
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
                .ExecuteAsync(new CancellationToken());

            SqlNonQueryResult deleteResult = await _databaseCommander.BuildCommand()
                .ForSqlNonQuery("DELETE FROM [dbo].[SampleTable] WHERE [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .ExecuteAsync(new CancellationToken());

            int rowCountAffectedFromInsert = insertResult.RowCountAffected;
            int rowCountAffectedFromDelete = deleteResult.RowCountAffected;

            Console.WriteLine("Row count affected from Insert: {0}", rowCountAffectedFromInsert);
            Console.WriteLine("Row count affected from Delete: {0}", rowCountAffectedFromDelete);
        }
    }
}
