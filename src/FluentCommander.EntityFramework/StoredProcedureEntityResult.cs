using System.Collections.Generic;
using System.Data;
using FluentCommander.StoredProcedure;

namespace FluentCommander.EntityFramework
{
    public class StoredProcedureEntityResult<TEntity> : StoredProcedureResult
    {
        public StoredProcedureEntityResult(List<DatabaseCommandParameter> parameters, DataTable dataTable, List<TEntity> result)
            : base(parameters, dataTable)
        {
            Result = result;
        }

        public List<TEntity> Result { get; set; }
    }
}
