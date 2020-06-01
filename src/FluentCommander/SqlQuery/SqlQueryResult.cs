using System.Data;
using FluentCommander.Core;

namespace FluentCommander.SqlQuery
{
    public class SqlQueryResult : DataTableResult
    {
        public SqlQueryResult(DataTable dataTable)
            : base(dataTable) { }
    }
}
