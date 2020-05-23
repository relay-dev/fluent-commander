using System.Data;

namespace FluentCommander.Database
{
    public class SqlQueryCommandResult : DataTableResult
    {
        public SqlQueryCommandResult(DataTable dataTable)
            : base(dataTable) { }
    }
}
