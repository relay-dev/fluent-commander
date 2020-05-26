using FluentCommander.Core;
using System.Data;

namespace FluentCommander
{
    public class SqlQueryResult : DataTableResult
    {
        public SqlQueryResult(DataTable dataTable)
            : base(dataTable) { }
    }
}
