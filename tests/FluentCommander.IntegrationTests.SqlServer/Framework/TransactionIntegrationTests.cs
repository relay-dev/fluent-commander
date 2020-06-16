using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentCommander.SqlNonQuery;
using Microsoft.Data.SqlClient;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.IntegrationTests.SqlServer.Framework
{
    [Collection("Service Provider collection")]
    public class TransactionIntegrationTests : SqlServerIntegrationTest<IDatabaseCommander>
    {
        public TransactionIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public async Task ExecuteSqlNonQueryCommandAsync_InsertDeleteWithInputParameters_ShouldInsertDeleteToTheDatabase()
        {
            // Arrange
            string sampleVarChar = "Temporary TransactionScope Row";
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            // Insert
            SqlNonQueryResult insertResult = await SUT.BuildCommand()
                .ForSqlNonQuery(InsertSql)
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .AddInputParameter("CreatedBy", TestUsername)
                .AddInputParameter("CreatedDate", Timestamp)
                .ExecuteAsync(cancellationToken);

            try
            {
                using var transaction = new SqlTransaction(null);

                await SUT.BuildCommand()
                    .ForSqlNonQuery("UPDATE [dbo].[SampleTable] SET [SampleVarChar] = 'Temporary TransactionScope Row: Version 2' WHERE [SampleTableID] = @SampleTableID")
                    .AddInputParameter("SampleTableID", insertResult.Id)
                    .Join(transaction)
                    .ExecuteAsync(cancellationToken);

                await SUT.BuildCommand()
                    .ForSqlNonQuery("UPDATE [dbo].[SampleTable] SET [SampleVarChar] = 'Temporary TransactionScope Row: Version 3' WHERE [SampleTableID] = @SampleTableID")
                    .AddInputParameter("SampleTableID", insertResult.Id)
                    .Join(transaction)
                    .ExecuteAsync(cancellationToken);

                throw new Exception("Testing");
            }
            catch (Exception e)
            {
                if (e.Message != "Testing")
                    throw;
            }

            string result = ExecuteScalar<string>($"SELECT [SampleVarChar] FROM [dbo].[SampleTable] WHERE [SampleTableID] = '{insertResult.Id}'");

            // Assert
            insertResult.ShouldNotBeNull();
            insertResult.RowCountAffected.ShouldBe(1);
            insertResult.Id.ShouldBeGreaterThan(0);
            result.ShouldNotBeNull();
            result.ShouldBe("Temporary TransactionScope Row");
        }

        private string InsertSql =>
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
     output INSERTED.SampleTableID
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
    }
}
