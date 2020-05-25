using System.Data;
using FluentCommander.Database.Core;

namespace FluentCommander.Database
{
    /// <summary>
    /// The result of a pagination command
    /// </summary>
    public class PaginationResult : DataTableResult
    {
        /// <summary>
        /// Created a new instance of a PagerResult
        /// </summary>
        /// <param name="dataTable">The dataTable to be accessible as the result of the pagination</param>
        /// <param name="totalCount">The total count of all records in the view</param>
        public PaginationResult(DataTable dataTable, int totalCount)
            : base(dataTable)
        {
            TotalCount = totalCount;
        }

        /// <summary>
        /// The total count of all records in the view
        /// </summary>
        public int TotalCount { get; }
    }
}
