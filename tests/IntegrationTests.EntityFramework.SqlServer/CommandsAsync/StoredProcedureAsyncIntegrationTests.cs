using FluentCommander.EntityFramework;
using FluentCommander.IntegrationTests.EntityFramework.SqlServer.Entities;
using FluentCommander.StoredProcedure;
using Shouldly;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.IntegrationTests.EntityFramework.SqlServer.CommandsAsync
{
    [Collection("Service Provider collection")]
    public class StoredProcedureAsyncIntegrationTests : EntityFrameworkSqlServerIntegrationTest<DatabaseCommanderDomainContext>
    {
        public StoredProcedureAsyncIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public async Task ExecuteStoredProcedureAsync_WithAllInputTypesAndTableResult_ShouldReturnDataTable()
        {
            // Arrange & Act
            StoredProcedureResult result = await SUT.BuildCommand()
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
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }

        [Fact]
        public async Task ExecuteStoredProcedureAsync_WithOutputParameter_ShouldHaveOutputInResult()
        {
            // Arrange
            string outputParameterName = "SampleOutputInt";

            // Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntOutput_NoResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName, SqlDbType.Int)
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.GetOutputParameter<int>(outputParameterName).ShouldBeGreaterThan(0);

            // Print result
            WriteLine("Output Parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
        }

        [Fact]
        public async Task ExecuteStoredProcedureAsync_WithInputMultipleOutputAndTableResult_ShouldReturnEverythingAsExpected()
        {
            // Arrange
            string outputParameterName1 = "SampleOutputInt";
            string outputParameterName2 = "SampleOutputVarChar";

            // Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_MultipleOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName1, SqlDbType.Int)
                .AddOutputParameter(outputParameterName2, SqlDbType.VarChar, 1000)
                .ExecuteAsync(new CancellationToken());

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
        public async Task ExecuteStoredProcedureAsync_WithInputOutputParameter_ShouldReturnTheOutputAsExpected()
        {
            // Arrange
            string outputParameterName = "SampleInputOutputInt";
            int inputValue = 1;

            // Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntInputOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(outputParameterName, inputValue)
                .ExecuteAsync(new CancellationToken());

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
        public async Task ExecuteStoredProcedureAsync_WithInputOutputParameterSpecifyingType_ShouldReturnTheOutputAsExpected()
        {
            // Arrange
            string outputParameterName = "SampleInputOutputInt";
            int inputValue = 1;

            // Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntInputOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(outputParameterName, inputValue)
                .ExecuteAsync(new CancellationToken());

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
        public async Task ExecuteStoredProcedureAsync_WithInputOutputParameterSpecifyingTypeAndSize_ShouldReturnTheOutputAsExpected()
        {
            // Arrange
            string outputParameterName = "SampleInputOutputVarChar";

            // Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_VarCharOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(outputParameterName, 1, SqlDbType.VarChar, 50)
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.GetOutputParameter<string>(outputParameterName).ShouldNotBeNullOrEmpty();

            // Print result
            WriteLine("Output Parameter: {0}", result.GetOutputParameter<string>(outputParameterName));
            WriteLine(result.DataTable);
        }

        [Fact]
        public async Task ExecuteStoredProcedureAsync_WithVarCharInput_ShouldPassTheSizeAsExpected()
        {
            // Arrange & Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_VarCharInput_NoOutput_TableResult]")
                .AddInputParameter("SampleVarChar", "Row 1", SqlDbType.VarChar, 1000)
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print result
            WriteLine(result.DataTable);
        }

        [Fact]
        public async Task ExecuteStoredProcedureAsync_WithReturnParameter_ShouldReturnAsExpected()
        {
            // Arrange & Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_NoInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.GetReturnParameter<int>().ShouldBeGreaterThan(0);

            // Print result
            WriteLine("Return Parameter: {0}", result.GetReturnParameter<int>());
        }

        [Fact]
        public async Task ExecuteStoredProcedureAsync_WithInputOutputAndReturn_ShouldReturnAsExpected()
        {
            // Arrange
            string outputParameterName = "SampleOutputBigInt";
            int inputValue = 2;

            // Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_Input_Output_ReturnBigInt]")
                .AddInputParameter("SampleTableID", 0)
                .AddInputOutputParameter(outputParameterName, inputValue)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

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
        public async Task ExecuteStoredProcedureAsync_WithOptionalInputParameter_ShouldReturnAsExpected()
        {
            // Arrange & Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_OptionalInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.GetReturnParameter<int>().ShouldBeGreaterThan(0);

            // Print result
            WriteLine("Return Parameter: {0}", result.GetReturnParameter<int>());
        }

        [Fact]
        public async Task ExecuteStoredProcedureAsync_WithOptionalInputParameterSpecifyingType_ShouldReturnAsExpected()
        {
            // Arrange & Act
            StoredProcedureResult result = await SUT.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_OptionalInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1, SqlDbType.BigInt)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.GetReturnParameter<int>().ShouldBeGreaterThan(0);

            // Print result
            WriteLine("Return Parameter: {0}", result.GetReturnParameter<int>());
        }
    }
}
