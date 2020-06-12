using FluentCommander;
using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Pagination;

namespace Samples.Commands
{
    /// <notes>
    /// This sample class demonstrates how to build command for a SQL pagination query
    /// </notes>
    [SampleFixture]
    public class PaginationSamples : SampleBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public PaginationSamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// Several defaults are specified so the only input required is the target
        /// </notes>
        [Sample(Key = "1")]
        public async Task ExecutePaginationUsingMinimalInput()
        {
            PaginationResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            int totalCount = result.TotalCount;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Total Row count: {0}", totalCount);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }

        /// <notes>
        /// Specific columns can be specified for selecting
        /// </notes>
        [Sample(Key = "2")]
        public async Task ExecutePaginationSelectingColumns()
        {
            PaginationResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .Select("[SampleInt]")
                .From("[dbo].[SampleTable]")
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            int totalCount = result.TotalCount;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Total Row count: {0}", totalCount);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }

        /// <notes>
        /// Filters can be applied
        /// </notes>
        [Sample(Key = "3")]
        public async Task ExecutePaginationFilteringRows()
        {
            PaginationResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .Where("[SampleTableID] = 1")
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            int totalCount = result.TotalCount;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Total Row count: {0}", totalCount);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }

        /// <notes>
        /// Ordering can be applied to rows
        /// </notes>
        [Sample(Key = "4")]
        public async Task ExecutePaginationOrderingRows()
        {
            PaginationResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .OrderBy("[SampleTableID] DESC")
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            int totalCount = result.TotalCount;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Total Row count: {0}", totalCount);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }

        /// <notes>
        /// Ordering can be applied to rows
        /// </notes>
        [Sample(Key = "5")]
        public async Task ExecutePaginationSettingPageSize()
        {
            PaginationResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .PageSize(10)
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            int totalCount = result.TotalCount;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Total Row count: {0}", totalCount);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }

        /// <notes>
        /// In this sample, all options are used
        /// </notes>
        [Sample(Key = "6")]
        public async Task ExecutePaginationAllSettingsAreUsed()
        {
            PaginationResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .Select("[SampleTableID]")
                .From("[dbo].[SampleTable]")
                .Where("[SampleTableID] < 100")
                .OrderBy("1")
                .PageSize(25)
                .PageNumber(2)
                .Timeout(TimeSpan.FromSeconds(30))
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            int totalCount = result.TotalCount;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Total Row count: {0}", totalCount);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }
    }
}
