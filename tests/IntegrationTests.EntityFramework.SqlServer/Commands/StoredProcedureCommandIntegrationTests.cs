using FluentCommander;
using FluentCommander.EntityFramework;
using IntegrationTests.EntityFramework.SqlServer.Entities;
using Shouldly;
using System.Data;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.EntityFramework.SqlServer.Commands
{
    [Collection("Service Provider collection")]
    public class StoredProcedureCommandIntegrationTests : EntityFrameworkSqlServerIntegrationTest<DatabaseCommanderDomainContext>
    {
        public StoredProcedureCommandIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        // TODO: 
        [Fact]
        public void ExecuteStoredProcedure_WithAllInputTypesAndTableResult_ShouldReturnDataTable()
        {
            // Arrange & Act
            StoredProcedureEntityResult<Sample> result = SUT.BuildCommand()
                .ForStoredProcedure<Sample>("[dbo].[usp_VarCharInput_NoOutput_TableResult]")
                .AddInputParameter("SampleVarChar", "Row 1")
                .Project(sample =>
                {
                    sample.Property(s => s.SampleId).MapFrom("SampleTableID");
                    sample.Property(s => s.ModifiedBy).Ignore();
                    sample.Property(s => s.ModifiedDate).Ignore();
                })
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.Result.ShouldNotBeNull();
            result.Result.Count.ShouldBeGreaterThan(0);
            result.Result.First().SampleId.ShouldBeGreaterThan(0);

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
    }
}
