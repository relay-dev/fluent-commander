using System.Collections.Generic;
using System.Data;
using FluentCommander.Core;

namespace FluentCommander.SqlQuery
{
    public class SqlQueryResult : DataTableResult
    {
        public SqlQueryResult(DataTable dataTable)
            : base(dataTable) { }
    }

    public class SqlQueryResult<TEntity>
    {
        public SqlQueryResult(List<TEntity> result)
        {
            Result = result;
        }

        /// <summary>
        /// The count of all records returned
        /// </summary>
        public int Count => Result?.Count ?? 0;

        /// <summary>
        /// The records returned for this iteration of the pager
        /// </summary>
        public List<TEntity> Result { get; }

        /// <summary>
        /// Indicates whether or not the DataTable has data in it
        /// </summary>
        public bool HasData => Count > 0;
    }
}
