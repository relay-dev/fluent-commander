using System.Data;

namespace FluentCommander.Core
{
    /// <summary>
    /// A command result that contains a DataTable
    /// </summary>
    public class DataTableResult
    {
        public DataTableResult(DataTable dataTable)
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
        public bool HasData => Count > 0;
    }
}
