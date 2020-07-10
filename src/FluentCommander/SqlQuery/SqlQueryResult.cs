using FluentCommander.Core;
using System.Collections.Generic;
using System.Data;

namespace FluentCommander.SqlQuery
{
    public class SqlQueryResult : DataTableResult
    {
        public SqlQueryResult(DataTable dataTable)
            : base(dataTable) { }
    }

    public class SqlQueryResult<TEntity>
    {
        public SqlQueryResult(List<TEntity> data)
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
