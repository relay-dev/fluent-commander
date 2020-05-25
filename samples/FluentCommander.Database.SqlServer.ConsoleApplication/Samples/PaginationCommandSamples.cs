using Core.Plugins.Extensions;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Samples
{
    /// <notes>
    /// This sample class demonstrates how to build command for a SQL pagination query
    /// </notes>
    [SampleFixture]
    public class PaginationCommandSamples : CommandSampleBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public PaginationCommandSamples(
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

            Console.WriteLine(result.DataTable.ToPrintFriendly());
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

            Console.WriteLine(result.DataTable.ToPrintFriendly());
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

            Console.WriteLine(result.DataTable.ToPrintFriendly());
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

            Console.WriteLine(result.DataTable.ToPrintFriendly());
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

            Console.WriteLine(result.DataTable.ToPrintFriendly());
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
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }
    }
}
