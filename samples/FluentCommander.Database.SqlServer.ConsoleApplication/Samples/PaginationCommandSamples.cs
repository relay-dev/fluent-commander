using ConsoleApplication.SqlServer.Framework;
using Core.Plugins.Extensions;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Samples
{
    /// <notes>
    /// This sample class demonstrates how to build command for a SQL pagination query
    /// </notes>
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
        private async Task ExecutePaginationUsingMinimalInput()
        {
            PaginationCommandResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Specific columns can be specified for selecting
        /// </notes>
        private async Task ExecutePaginationSelectingColumns()
        {
            PaginationCommandResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .Select("[SampleInt]")
                .From("[dbo].[SampleTable]")
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Filters can be applied
        /// </notes>
        private async Task ExecutePaginationFilteringRows()
        {
            PaginationCommandResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .Where("[SampleTableID] = 1")
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Ordering can be applied to rows
        /// </notes>
        private async Task ExecutePaginationOrderingRows()
        {
            PaginationCommandResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .OrderBy("[SampleTableID] DESC")
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Ordering can be applied to rows
        /// </notes>
        private async Task ExecutePaginationSettingPageSize()
        {
            PaginationCommandResult result = await _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .PageSize(10)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// In this sample, all options are used
        /// </notes>
        private async Task ExecutePaginationAllSettingsAreUsed()
        {
            PaginationCommandResult result = await _databaseCommander.BuildCommand()
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

        protected override void Init()
        {
            SampleMethods = new List<SampleMethodAsync>
            {
                new SampleMethodAsync("1", "ExecutePaginationUsingMinimalInput()", async () => await ExecutePaginationUsingMinimalInput()),
                new SampleMethodAsync("2", "ExecutePaginationSelectingColumns()", async () => await ExecutePaginationSelectingColumns()),
                new SampleMethodAsync("3", "ExecutePaginationFilteringRows()", async () => await ExecutePaginationFilteringRows()),
                new SampleMethodAsync("4", "ExecutePaginationOrderingRows()", async () => await ExecutePaginationOrderingRows()),
                new SampleMethodAsync("5", "ExecutePaginationSettingPageSize()", async () => await ExecutePaginationSettingPageSize()),
                new SampleMethodAsync("6", "ExecutePaginationAllSettingsAreUsed()", async () => await ExecutePaginationAllSettingsAreUsed())
            };
        }
    }
}
