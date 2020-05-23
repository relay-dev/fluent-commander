using System.Data;

namespace FluentCommander.Database
{
    public class PaginationCommandResult : DataTableResult
    {
        public PaginationCommandResult(PaginationResult paginationResult)
            : base(paginationResult.DataTable)
        {
            TotalCount = paginationResult.TotalCount;
        }

        public PaginationCommandResult(DataTable dataTable, int totalCount)
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
