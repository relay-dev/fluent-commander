using System.Data;

namespace FluentCommander.Database
{
    public class PaginationCommandResult
    {
        public PaginationCommandResult(PaginationResult paginationResult)
        {
            DataTable = paginationResult.DataTable;
            TotalCount = paginationResult.TotalCount;
        }

        public PaginationCommandResult(DataTable dataTable, int totalCount)
        {
            DataTable = dataTable;
            TotalCount = totalCount;
        }

        /// <summary>
        /// The count of all records returned
        /// </summary>
        public int Count => DataTable.Rows.Count;

        /// <summary>
        /// The records returned for this iteration of the pager
        /// </summary>
        public DataTable DataTable { get; }

        /// <summary>
        /// Indicates whether or not the DataTable has data in it
        /// </summary>
        public bool HasData => DataTable != null && DataTable.Rows.Count > 0;

        /// <summary>
        /// The total count of all records in the view
        /// </summary>
        public int TotalCount { get; }
    }
}
