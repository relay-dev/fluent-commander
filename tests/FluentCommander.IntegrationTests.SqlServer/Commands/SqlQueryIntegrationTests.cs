using FluentCommander.Samples.Setup.Entities;
using FluentCommander.SqlQuery;
using Shouldly;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.IntegrationTests.SqlServer.Commands
{
    [Collection("Service Provider collection")]
    public class SqlQueryIntegrationTests : SqlServerIntegrationTest<IDatabaseCommander>
    {
        public SqlQueryIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public void ExecuteSqlQueryCommand_WithInputParameters_ShouldReturnDataTable()
        {
            // Arrange & Act
            SqlQueryResult result = SUT.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecuteSqlQueryCommandAsync_WithBehaviorsSet_ShouldRespectBehaviorSettings()
        {
            // Arrange & Act
            SqlQueryResult result = SUT.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .Behaviors(behavior => behavior.SingleResult().KeyInfo())
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecuteSqlQueryCommandAsync_WithOptionsSet_ShouldRespectOptionsSettings()
        {
            // Arrange & Act
            SqlQueryResult result = SUT.BuildCommand()
                .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .Options(option => option.OpenConnectionWithoutRetry())
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecuteSqlQueryCommand_WithProjectionMap_ShouldReturnListOfEntities()
        {
            // Arrange & Act
            SqlQueryResult<SampleEntity> result = SUT.BuildCommand()
                .ForSqlQuery<SampleEntity>("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .Project(sample =>
                {
                    sample.Property(s => s.SampleId).MapFrom("SampleTableID");
                    sample.Property(s => s.SampleInt).MapFrom("SampleInt");
                    sample.Property(s => s.SampleSmallInt).MapFrom("SampleSmallInt");
                    sample.Property(s => s.SampleTinyInt).MapFrom("SampleTinyInt");
                    sample.Property(s => s.SampleBit).MapFrom("SampleBit");
                    sample.Property(s => s.SampleDecimal).MapFrom("SampleDecimal");
                    sample.Property(s => s.SampleFloat).MapFrom("SampleFloat");
                    sample.Property(s => s.SampleDateTime).MapFrom("SampleDateTime");
                    sample.Property(s => s.SampleUniqueIdentifier).MapFrom("SampleUniqueIdentifier");
                    sample.Property(s => s.SampleVarChar).MapFrom("SampleVarChar");
                    sample.Property(s => s.CreatedBy).MapFrom("CreatedBy");
                    sample.Property(s => s.CreatedDate).MapFrom("CreatedDate");
                    sample.Property(s => s.ModifiedBy).MapFrom("ModifiedBy");
                    sample.Property(s => s.ModifiedDate).MapFrom("ModifiedDate");
                })
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Count.ShouldBeGreaterThan(0);
            result.Data.First().SampleId.ShouldBeGreaterThan(0);
            result.Data.First().SampleDateTime.ShouldNotBe(DateTime.MinValue);
            result.Data.First().SampleUniqueIdentifier.ShouldNotBe(Guid.Empty);
            result.Data.First().SampleVarChar.ShouldNotBeNullOrEmpty("SampleVarChar");

            // Print result
            WriteLine(result.Data);
        }
    }
}
