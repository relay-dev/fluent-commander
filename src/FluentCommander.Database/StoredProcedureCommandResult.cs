using System.Collections.Generic;
using System.Data;

namespace FluentCommander.Database
{
    public class StoredProcedureCommandResult : StoredProcedureResult
    {
        public StoredProcedureCommandResult(StoredProcedureResult storedProcedureResult)
            : base(storedProcedureResult.Parameters, storedProcedureResult.DataTable) { }

        public StoredProcedureCommandResult(List<DatabaseCommandParameter> parameters, DataTable dataTable) 
            : base(parameters, dataTable) { }
    }
}
