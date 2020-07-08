using FluentCommander.SqlNonQuery;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
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
            string sampleVarChar = $"Temporary Transaction Row - {Guid.NewGuid()}";
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

            long sampleTableId = ExecuteScalar<long>($"SELECT [SampleTableID] FROM [dbo].[SampleTable] WHERE [SampleVarChar] = '{sampleVarChar}'");

            try
            {
                using var transaction = ResolveService<IDatabaseCommanderTransactionFactory>().Create();

                await SUT.BuildCommand()
                    .ForSqlNonQuery("UPDATE [dbo].[SampleTable] SET [SampleVarChar] = @SampleVarChar WHERE [SampleTableID] = @SampleTableID")
                    .AddInputParameter("SampleTableID", sampleTableId)
                    .AddInputParameter("SampleVarChar", sampleVarChar + "(Version 2)")
                    .Join(transaction)
                    .ExecuteAsync(cancellationToken);

                await SUT.BuildCommand()
                    .ForSqlNonQuery("UPDATE [dbo].[SampleTable] SET [SampleVarChar] = @SampleVarChar WHERE [SampleTableID] = @SampleTableID")
                    .AddInputParameter("SampleTableID", sampleTableId)
                    .AddInputParameter("SampleVarChar", sampleVarChar + "(Version 3)")
                    .Join(transaction)
                    .ExecuteAsync(cancellationToken);

                throw new Exception("Testing");
            }
            catch (Exception e)
            {
                if (e.Message != "Testing")
                    throw;
            }

            string result = ExecuteScalar<string>($"SELECT [SampleVarChar] FROM [dbo].[SampleTable] WHERE [SampleTableID] = {sampleTableId}");

            // Assert
            insertResult.ShouldNotBeNull();
            insertResult.RowCountAffected.ShouldBe(1);
            result.ShouldNotBeNull();
            result.ShouldBe("Temporary Transaction Row");
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
