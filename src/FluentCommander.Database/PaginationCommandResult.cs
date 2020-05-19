using System.Data;

namespace FluentCommander.Database
{
    public class PaginationCommandResult
    {
        public DataTable DataTable { get; }
        public int TotalCount { get; }
        public bool HasData => DataTable != null && DataTable.Rows.Count > 0;

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
    }
}
