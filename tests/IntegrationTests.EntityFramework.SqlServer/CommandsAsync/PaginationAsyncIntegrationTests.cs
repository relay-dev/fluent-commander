using FluentCommander;
using FluentCommander.EntityFramework;
using IntegrationTests.EntityFramework.SqlServer.Entities;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Pagination;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.EntityFramework.SqlServer.CommandsAsync
{
    [Collection("Service Provider collection")]
    public class PaginationAsyncIntegrationTests : EntityFrameworkSqlServerIntegrationTest<DatabaseCommanderDomainContext>
    {
        public PaginationAsyncIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public async Task ExecutePaginationAsync_ShouldReturnRowsAsExpected_WhenUsingMinimalInput()
        {
            // Arrange && Act
            PaginationResult result = await SUT.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.TotalCount.ShouldBeGreaterThan(0);

            // Print
            WriteLine("Total Count: {0}", result.TotalCount);
            WriteLine(result.DataTable);
        }

        [Fact]
        public async Task ExecutePaginationAsync_ShouldReturnRowsAsExpected_WhenSelectingColumns()
        {
            // Arrange && Act
            PaginationResult result = await SUT.BuildCommand()
                .ForPagination()
                .Select("[SampleInt]")
                .From("[dbo].[SampleTable]")
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.TotalCount.ShouldBeGreaterThan(0);
            result.DataTable.Columns.Count.ShouldBe(1);

            // Print
            WriteLine("Total Count: {0}", result.TotalCount);
            WriteLine(result.DataTable);
        }

        [Fact]
        public async Task ExecutePaginationAsync_ShouldReturnRowsAsExpected_WhenFilteringRows()
        {
            // Arrange && Act
            PaginationResult result = await SUT.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .Where("[SampleTableID] = 1")
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.TotalCount.ShouldBeGreaterThan(0);
            result.DataTable.Rows.Count.ShouldBe(1);

            // Print
            WriteLine("Total Count: {0}", result.TotalCount);
            WriteLine(result.DataTable);
        }

        [Fact]
        public async Task ExecutePaginationAsync_ShouldReturnRowsAsExpected_WhenOrderingRows()
        {
            // Arrange && Act
            PaginationResult result = await SUT.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .OrderBy("[SampleTableID] DESC")
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.TotalCount.ShouldBeGreaterThan(0);
            result.DataTable.Rows[0]["SampleTableID"].ShouldNotBeNull();
            Convert.ToInt64(result.DataTable.Rows[0]["SampleTableID"].ToString()).ShouldBe(ExecuteScalar<long>("SELECT MAX([SampleTableID]) FROM [dbo].[SampleTable]"));

            // Print
            WriteLine("Total Count: {0}", result.TotalCount);
            WriteLine(result.DataTable);
        }

        [Fact]
        public async Task ExecutePaginationAsync_ShouldReturnRowsAsExpected_WhenSettingPageSize()
        {
            // Arrange
            const int pageSize = 10;

            // Act
            PaginationResult result = await SUT.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .PageSize(pageSize)
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.TotalCount.ShouldBeGreaterThan(0);
            result.DataTable.Rows.Count.ShouldBe(pageSize);

            // Print
            WriteLine("Total Count: {0}", result.TotalCount);
            WriteLine(result.DataTable);
        }

        [Fact]
        public async Task ExecutePaginationAsync_ShouldReturnRowsAsExpected_WhenAllSettingsAreUsed()
        {
            // Arrange
            const int pageSize = 25;
            const int pageNumber = 2;

            // Act
            PaginationResult result = await SUT.BuildCommand()
                .ForPagination()
                .Select("[SampleTableID]")
                .From("[dbo].[SampleTable]")
                .Where("[SampleTableID] < 100")
                .OrderBy("1")
                .PageSize(pageSize)
                .PageNumber(pageNumber)
                .ExecuteAsync(new CancellationToken());

            // Assert
            result.ShouldNotBeNull();
            result.HasData.ShouldBeTrue();
            result.TotalCount.ShouldBeGreaterThan(0);
            result.DataTable.Columns.Count.ShouldBe(1);
            result.DataTable.Rows.Count.ShouldBe(pageSize);
            result.DataTable.Rows[0][0].ShouldNotBeNull();
            Convert.ToInt32(result.DataTable.Rows[0][0]).ShouldBe(pageSize + 1);
            Convert.ToInt32(result.DataTable.Rows[pageSize - 1][0]).ShouldBe(pageSize * pageNumber);

            // Print
            WriteLine("Total Count: {0}", result.TotalCount);
            WriteLine(result.DataTable);
        }
    }
}
