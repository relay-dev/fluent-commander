using FluentCommander.Database;
using Shouldly;
using System;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.SqlServer.Commands
{
    [Collection("Service Provider collection")]
    public class PaginationCommandIntegrationTests : IntegrationTest<IDatabaseCommander>
    {
        public PaginationCommandIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public void ExecutePagination_ShouldReturnRowsAsExpected_WhenUsingMinimalInput()
        {
            // Arrange && Act
            PaginationCommandResult result = SUT.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();

            // Print
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecutePagination_ShouldReturnRowsAsExpected_WhenSelectingColumns()
        {
            // Arrange && Act
            PaginationCommandResult result = SUT.BuildCommand()
                .ForPagination()
                .Select("[SampleInt]")
                .From("[dbo].[SampleTable]")
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.DataTable.Columns.Count.ShouldBe(1);

            // Print
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecutePagination_ShouldReturnRowsAsExpected_WhenFilteringRows()
        {
            // Arrange && Act
            PaginationCommandResult result = SUT.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .Where("[SampleTableID] = 1")
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.DataTable.Rows.Count.ShouldBe(1);

            // Print
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecutePagination_ShouldReturnRowsAsExpected_WhenOrderingRows()
        {
            // Arrange && Act
            PaginationCommandResult result = SUT.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .OrderBy("[SampleTableID] DESC")
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.DataTable.Rows[0]["SampleTableID"].ShouldNotBeNull();
            Convert.ToInt64(result.DataTable.Rows[0]["SampleTableID"].ToString()).ShouldBe(ExecuteScalar<long>("SELECT MAX([SampleTableID]) FROM [dbo].[SampleTable]"));

            // Print
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecutePagination_ShouldReturnRowsAsExpected_WhenSettingPageSize()
        {
            // Arrange
            const int pageSize = 10;

            // Act
            PaginationCommandResult result = SUT.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .PageSize(pageSize)
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.DataTable.Rows.Count.ShouldBe(pageSize);

            // Print
            WriteLine(result.DataTable);
        }

        [Fact]
        public void ExecutePagination_ShouldReturnRowsAsExpected_WhenAllSettingsAreUsed()
        {
            // Arrange
            const int pageSize = 25;
            const int pageNumber = 2;

            // Act
            PaginationCommandResult result = SUT.BuildCommand()
                .ForPagination()
                .Select("[SampleTableID]")
                .From("[dbo].[SampleTable]")
                .Where("[SampleTableID] < 100")
                .OrderBy("1")
                .PageSize(pageSize)
                .PageNumber(pageNumber)
                .Execute();

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.DataTable.Columns.Count.ShouldBe(1);
            result.DataTable.Rows.Count.ShouldBe(pageSize);
            result.DataTable.Rows[0][0].ShouldNotBeNull();
            Convert.ToInt32(result.DataTable.Rows[0][0]).ShouldBe(pageSize + 1);
            Convert.ToInt32(result.DataTable.Rows[pageSize - 1][0]).ShouldBe(pageSize * pageNumber);

            // Print
            WriteLine(result.DataTable);
        }
    }
}
