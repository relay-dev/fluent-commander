using System.Collections.Generic;
using System.Data;

namespace FluentCommander.Database
{
    public class StoredProcedureCommandResult : SqlQueryCommandResult
    {
        public StoredProcedureCommandResult(List<DatabaseCommandParameter> parameters, DataTable dataTable)
            : base(parameters, dataTable) { }
    }
}
