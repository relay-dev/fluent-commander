using FluentCommander;
using FluentCommander.StoredProcedure;
using Setup.Entities;
using Shouldly;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.SqlServer.Commands
{
    [Collection("Service Provider collection")]
    public class StoredProcedureEntityIntegrationTests : SqlServerIntegrationTest<IDatabaseCommander>
    {
        public StoredProcedureEntityIntegrationTests(ServiceProviderFixture serviceProviderFixture,
            ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public void ExecuteStoredProcedure_WithAllInputTypesAndTableResult_ShouldReturnListOfEntities()
        {
            // Arrange & Act
            StoredProcedureResult<SampleEntity> result = SUT.BuildCommand()
                .ForStoredProcedure<SampleEntity>("[dbo].[usp_VarCharInput_NoOutput_TableResult]")
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
            result.Result.ShouldNotBeNull();
            result.Result.Count.ShouldBeGreaterThan(0);
            result.Result.First().SampleId.ShouldBeGreaterThan(0);
            result.Result.First().SampleDateTime.ShouldNotBe(DateTime.MinValue);
            result.Result.First().SampleUniqueIdentifier.ShouldNotBe(Guid.Empty);
            result.Result.First().SampleVarChar.ShouldNotBeNullOrEmpty("SampleVarChar");

            // Print result
            WriteLine(result.Result);
        }
    }
}
