using FluentCommander.Samples.Setup.Entities;
using FluentCommander.StoredProcedure;
using Shouldly;
using System;
using System.Data;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.IntegrationTests.SqlServer.Commands
{
    [Collection("Service Provider collection")]
    public class StoredProcedureIntegrationTests : SqlServerIntegrationTest<IDatabaseCommander>
    {
        public StoredProcedureIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public void ExecuteStoredProcedure_WithAllInputTypesAndTableResult_ShouldReturnDataTable()
        {
            // Arrange & Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_AllInputTypes_NoOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleInt", 0)
                .AddInputParameter("SampleSmallInt", 0)
                .AddInputParameter("SampleTinyInt", 0)
                .AddInputParameter("SampleBit", false)
                .AddInputParameter("SampleDecimal", 0)
                .AddInputParameter("SampleFloat", 0)
                .AddInputParameter("SampleDateTime", DateTime.Now)
                .AddInputParameter("SampleUniqueIdentifier", Guid.NewGuid())
                .AddInputParameter("SampleVarChar", "Row 1")
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecuteStoredProcedure_WithOutputParameter_ShouldHaveOutputInResult()
        {
            // Arrange
            string outputParameterName = "SampleOutputInt";

            // Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntOutput_NoResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName, SqlDbType.Int)
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.GetOutputParameter<int>(outputParameterName).ShouldBeGreaterThan(0);

            // Print result
            WriteLine("Output Parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
        }

        [Fact]
        public void ExecuteStoredProcedure_WithInputMultipleOutputAndTableResult_ShouldReturnEverythingAsExpected()
        {
            // Arrange
            string outputParameterName1 = "SampleOutputInt";
            string outputParameterName2 = "SampleOutputVarChar";

            // Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_MultipleOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName1, SqlDbType.Int)
                .AddOutputParameter(outputParameterName2, SqlDbType.VarChar, 1000)
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.GetOutputParameter<int>(outputParameterName1).ShouldBeGreaterThan(0);
            result.GetOutputParameter<string>(outputParameterName2).ShouldNotBeNullOrEmpty();

            // Print result
            WriteLine("Output Parameter 1: {0}", result.GetOutputParameter<int>(outputParameterName1));
            WriteLine("Output Parameter 2: {0}", result.GetOutputParameter<string>(outputParameterName2));
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecuteStoredProcedure_WithInputOutputParameter_ShouldReturnTheOutputAsExpected()
        {
            // Arrange
            string outputParameterName = "SampleInputOutputInt";
            int inputValue = 1;

            // Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntInputOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(outputParameterName, inputValue)
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.GetOutputParameter<int>(outputParameterName).ShouldBeGreaterThan(0);
            result.GetOutputParameter<int>(outputParameterName).ShouldNotBe(inputValue);

            // Print result
            WriteLine("Output Parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecuteStoredProcedure_WithInputOutputParameterSpecifyingType_ShouldReturnTheOutputAsExpected()
        {
            // Arrange
            string outputParameterName = "SampleInputOutputInt";
            int inputValue = 1;

            // Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntInputOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(outputParameterName, inputValue, SqlDbType.Int)
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.GetOutputParameter<int>(outputParameterName).ShouldBeGreaterThan(0);
            result.GetOutputParameter<int>(outputParameterName).ShouldNotBe(inputValue);

            // Print result
            WriteLine("Output Parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecuteStoredProcedure_WithInputOutputParameterSpecifyingTypeAndSize_ShouldReturnTheOutputAsExpected()
        {
            // Arrange
            string outputParameterName = "SampleInputOutputVarChar";

            // Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_VarCharOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(outputParameterName, 1, SqlDbType.VarChar, 50)
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.GetOutputParameter<string>(outputParameterName).ShouldNotBeNullOrEmpty();

            // Print result
            WriteLine("Output Parameter: {0}", result.GetOutputParameter<string>(outputParameterName));
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecuteStoredProcedureAsync_WithInputOutputParameterSpecifyingTypeAndSize_ShouldReturnTheOutputAsExpected()
        {
            // Arrange & Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_VarCharInput_NoOutput_TableResult]")
                .AddInputParameter("SampleVarChar", "Row 1", SqlDbType.VarChar, 1000)
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecuteStoredProcedure_WithReturnParameter_ShouldReturnAsExpected()
        {
            // Arrange & Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_NoInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.GetReturnParameter<int>().ShouldBeGreaterThan(0);

            // Print result
            WriteLine("Return Parameter: {0}", result.GetReturnParameter<int>());
        }

        [Fact]
        public void ExecuteStoredProcedure_WithInputOutputAndReturn_ShouldReturnAsExpected()
        {
            // Arrange
            string outputParameterName = "SampleOutputBigInt";
            int inputValue = 2;

            // Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_Input_Output_ReturnBigInt]")
                .AddInputParameter("SampleTableID", 0)
                .AddInputOutputParameter(outputParameterName, inputValue)
                .WithReturnParameter()
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.GetOutputParameter<int>(outputParameterName).ShouldBeGreaterThan(0);
            result.GetOutputParameter<int>(outputParameterName).ShouldNotBe(inputValue);
            result.GetReturnParameter<int>().ShouldBeGreaterThan(0);

            // Print result
            WriteLine("Output Parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
            WriteLine("Return Parameter: {0}", result.GetReturnParameter<int>());
        }

        [Fact]
        public void ExecuteStoredProcedure_WithOptionalInputParameter_ShouldReturnAsExpected()
        {
            // Arrange & Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_OptionalInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.GetReturnParameter<int>().ShouldBeGreaterThan(0);

            // Print result
            WriteLine("Return Parameter: {0}", result.GetReturnParameter<int>());
        }

        [Fact]
        public void ExecuteStoredProcedure_WithOptionalInputParameterSpecifyingType_ShouldReturnAsExpected()
        {
            // Arrange & Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_OptionalInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1, SqlDbType.BigInt)
                .WithReturnParameter()
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.GetReturnParameter<int>().ShouldBeGreaterThan(0);

            // Print result
            WriteLine("Return Parameter: {0}", result.GetReturnParameter<int>());
        }

        [Fact]
        public void ExecuteStoredProcedureAsync_WithBehaviorsSet_ShouldRespectBehaviorSettings()
        {
            // Arrange & Act
            StoredProcedureResult result = SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_VarCharInput_NoOutput_TableResult]")
                .AddInputParameter("SampleVarChar", "Row 1", SqlDbType.VarChar, 1000)
                .Behaviors(behavior => behavior.SingleResult().KeyInfo())
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }

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
