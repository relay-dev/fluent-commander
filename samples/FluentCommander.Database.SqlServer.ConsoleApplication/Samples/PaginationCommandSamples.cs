using ConsoleApplication.SqlServer.Framework;
using Core.Plugins.Extensions;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

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
        private void ExecutePaginationUsingMinimalInput()
        {
            PaginationCommandResult result = _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .Execute();

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Specific columns can be specified for selecting
        /// </notes>
        private void ExecutePaginationSelectingColumns()
        {
            PaginationCommandResult result = _databaseCommander.BuildCommand()
                .ForPagination()
                .Select("[SampleInt]")
                .From("[dbo].[SampleTable]")
                .Execute();

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Filters can be applied
        /// </notes>
        private void ExecutePaginationFilteringRows()
        {
            PaginationCommandResult result = _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .Where("[SampleTableID] = 1")
                .Execute();

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Ordering can be applied to rows
        /// </notes>
        private void ExecutePaginationOrderingRows()
        {
            PaginationCommandResult result = _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .OrderBy("[SampleTableID] DESC")
                .Execute();

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Ordering can be applied to rows
        /// </notes>
        private void ExecutePaginationSettingPageSize()
        {
            PaginationCommandResult result = _databaseCommander.BuildCommand()
                .ForPagination()
                .From("[dbo].[SampleTable]")
                .PageSize(10)
                .Execute();

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// In this sample, all options are used
        /// </notes>
        private void ExecutePaginationAllSettingsAreUsed()
        {
            PaginationCommandResult result = _databaseCommander.BuildCommand()
                .ForPagination()
                .Select("[SampleTableID]")
                .From("[dbo].[SampleTable]")
                .Where("[SampleTableID] < 100")
                .OrderBy("1")
                .PageSize(25)
                .PageNumber(2)
                .Execute();

            Console.WriteLine(result.DataTable.ToPrintFriendly());
        }

        protected override void Init()
        {
            SampleMethods = new List<SampleMethod>
            {
                new SampleMethod("1", "ExecutePaginationUsingMinimalInput()", ExecutePaginationUsingMinimalInput),
                new SampleMethod("2", "ExecutePaginationSelectingColumns()", ExecutePaginationSelectingColumns),
                new SampleMethod("3", "ExecutePaginationFilteringRows()", ExecutePaginationFilteringRows),
                new SampleMethod("4", "ExecutePaginationOrderingRows()", ExecutePaginationOrderingRows),
                new SampleMethod("5", "ExecutePaginationSettingPageSize()", ExecutePaginationSettingPageSize),
                new SampleMethod("6", "ExecutePaginationAllSettingsAreUsed()", ExecutePaginationAllSettingsAreUsed)
            };
        }
    }
}
