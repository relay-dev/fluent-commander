using System.Data;

namespace FluentCommander.Database
{
    public class SqlQueryCommandResult
    {
        public SqlQueryCommandResult(DataTable dataTable)
        {
            DataTable = dataTable;
        }

        /// <summary>
        /// The count of all records returned
        /// </summary>
        public int Count => DataTable.Rows.Count;

        /// <summary>
        /// The records returned from the query
        /// </summary>
        public DataTable DataTable { get; }

        /// <summary>
        /// Indicates whether or not the DataTable has data in it
        /// </summary>
        public bool HasData => DataTable != null && DataTable.Rows.Count > 0;
    }
}
