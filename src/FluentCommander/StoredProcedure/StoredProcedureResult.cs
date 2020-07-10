using System.Collections.Generic;
using System.Data;

namespace FluentCommander.StoredProcedure
{
    public class StoredProcedureResult : StoredProcedureResultBase
    {
        public StoredProcedureResult(DataTable dataTable, List<DatabaseCommandParameter> parameters)
            : base(parameters)
        {
            DataTable = dataTable;
        }

        /// <summary>
        /// The count of all records returned
        /// </summary>
        public int Count => DataTable?.Rows.Count ?? 0;

        /// <summary>
        /// The records returned for this iteration of the pager
        /// </summary>
        public DataTable DataTable { get; }

        /// <summary>
        /// Indicates whether or not the DataTable has data in it
        /// </summary>
        public bool HasData => DataTable != null && DataTable.Rows.Count > 0;
    }

    public class StoredProcedureResult<TEntity> : StoredProcedureResultBase
    {
        public StoredProcedureResult(List<TEntity> data, List<DatabaseCommandParameter> parameters)
            : base(parameters)
        {
            Data = data;
        }

        /// <summary>
        /// The count of all records returned
        /// </summary>
        public int Count => Data?.Count ?? 0;

        /// <summary>
        /// The records returned for this iteration of the pager
        /// </summary>
        public List<TEntity> Data { get; }

        /// <summary>
        /// Indicates whether or not the DataTable has data in it
        /// </summary>
        public bool HasData => Count > 0;
    }
}
