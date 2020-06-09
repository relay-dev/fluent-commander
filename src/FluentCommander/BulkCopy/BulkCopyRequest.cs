using FluentCommander.Core;
using System.Data;
using FluentCommander.Core.Mapping;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyRequest : DatabaseCommandRequest, IHaveColumnMapping
    {
        public int BatchSize { get; set; }

        /// <summary>
        /// Optional; maps the dataTable column names to the database table column names
        /// </summary>
        public ColumnMapping ColumnMapping { get; set; }

        /// <summary>
        /// The dataTable that contains the data to be copied
        /// </summary>
        public DataTable DataTable { get; set; }


        public bool EnableStreaming { get; set; }

        public int NotifyAfter { get; set; }

        //public SqlBulkCopyOptions SqlBulkCopyOptions { get; set; }

        /// <summary>
        /// The name of the table to insert into
        /// </summary>
        public string TableName { get; set; }
    }
}
