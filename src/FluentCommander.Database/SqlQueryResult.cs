using FluentCommander.Database.Core;
using System.Data;

namespace FluentCommander.Database
{
    public class SqlQueryResult : DataTableResult
    {
        public SqlQueryResult(DataTable dataTable)
            : base(dataTable) { }
    }
}
