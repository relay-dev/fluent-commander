using FluentCommander.SqlNonQuery;
using Shouldly;
using System;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.IntegrationTests.SqlServer.Commands
{
    [Collection("Service Provider collection")]
    public class SqlNonQueryIntegrationTests : SqlServerIntegrationTest<IDatabaseCommander>
    {
        public SqlNonQueryIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public void ExecuteSqlNonQueryCommand_UpdateWithInputParameters_ShouldUpdateTheDatabase()
        {
            // Arrange
            int sampleTableId = 1;
            Guid newGuid = Guid.NewGuid();
            Guid oldGuid = ExecuteScalar<Guid>($"SELECT [SampleUniqueIdentifier] FROM [dbo].[SampleTable] WHERE [SampleTableID] = {sampleTableId}");

            // Act
            SqlNonQueryResult result = SUT.BuildCommand()
                .ForSqlNonQuery("UPDATE [dbo].[SampleTable] SET [SampleUniqueIdentifier] = @NewGuid, [ModifiedBy] = @ModifiedBy, [ModifiedDate] = @ModifiedDate WHERE [SampleTableID] = @SampleTableID")
                .AddInputParameter("SampleTableID", sampleTableId)
                .AddInputParameter("NewGuid", newGuid)
                .AddInputParameter("ModifiedBy", TestUsername)
                .AddInputParameter("ModifiedDate", Timestamp)
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            Guid actualGuid = ExecuteScalar<Guid>($"SELECT [SampleUniqueIdentifier] FROM [dbo].[SampleTable] WHERE [SampleTableID] = {sampleTableId}");
            actualGuid.ShouldBe(newGuid);
            actualGuid.ShouldNotBe(oldGuid);

            // Print result
            WriteLine("Old GUID: {0}", oldGuid);
            WriteLine("New GUID: {0}", newGuid);
        }

        [Fact]
        public void ExecuteSqlNonQueryCommand_InsertDeleteWithInputParameters_ShouldInsertDeleteToTheDatabase()
        {
            // Arrange
            string sampleVarChar = "Temporary Row";

            // Act
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

            SqlNonQueryResult insertResult = SUT.BuildCommand()
                .ForSqlNonQuery(insertSql)
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .AddInputParameter("CreatedBy", TestUsername)
                .AddInputParameter("CreatedDate", Timestamp)
                .Execute();

            ExecuteScalar<int>($"SELECT COUNT(1) FROM [dbo].[SampleTable] WHERE [SampleVarChar] = '{sampleVarChar}'").ShouldBe(1);

            SqlNonQueryResult deleteResult = SUT.BuildCommand()
                .ForSqlNonQuery("DELETE FROM [dbo].[SampleTable] WHERE [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleVarChar", sampleVarChar)
                .Execute();

            // Assert
            insertResult.ShouldNotBeNull();
            insertResult.RowCountAffected.ShouldBe(1);
            deleteResult.ShouldNotBeNull();
            deleteResult.RowCountAffected.ShouldBe(1);
            ExecuteScalar<int>($"SELECT COUNT(1) FROM [dbo].[SampleTable] WHERE [SampleVarChar] = '{sampleVarChar}'").ShouldBe(0);

            // Print result
            WriteLine("Insert Row Count Affected: {0}", insertResult.RowCountAffected);
            WriteLine("Delete Row Count Affected: {0}", deleteResult.RowCountAffected);
        }
    }
}
