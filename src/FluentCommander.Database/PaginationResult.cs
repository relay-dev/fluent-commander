using System.Data;

namespace FluentCommander.Database
{
    /// <summary>
    /// The result of a pagination command
    /// </summary>
    public class PaginationResult
    {
        /// <summary>
        /// Created a new instance of a PagerResult
        /// </summary>
        /// <param name="dataTable">The dataTable to be accessible as the result of the pagination</param>
        /// <param name="totalCount">The total count of all records in the view</param>
        public PaginationResult(DataTable dataTable, int totalCount)
        {
            DataTable = dataTable;
            TotalCount = totalCount;
        }

        /// <summary>
        /// The count of all records returned
        /// </summary>
        public int Count => DataTable.Rows.Count;

        /// <summary>
        /// The total count of all records in the view
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// The records returned for this iteration of the pager
        /// </summary>
        public DataTable DataTable { get; }
    }
}
