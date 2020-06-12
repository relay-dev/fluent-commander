using FluentCommander.Core;
using FluentCommander.Core.Mapping;
using System;
using System.Data;
using System.Data.Common;
using MappingType = FluentCommander.Core.Mapping.MappingType;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyRequest : DatabaseCommandRequest, IColumnMappingRequest
    {
        /// <summary>
        /// Number of rows in each batch. At the end of each batch, the rows in the batch are sent to the server.
        /// </summary>
        public int? BatchSize { get; set; }

        /// <summary>
        /// Column mappings define the relationships between columns in the data source and columns in the destination.
        /// </summary>
        public ColumnMapping ColumnMapping { get; set; }

        /// <summary>
        /// The DataReader to stream the data to be copied
        /// </summary>
        public IDataReader DataReader { get; set; }

        /// <summary>
        /// The DataRows that contains the data to be copied
        /// </summary>
        public DataRow[] DataRows { get; set; }

        /// <summary>
        /// Only rows matching the row state are copied to the destination
        /// </summary>
        public DataRowState? DataRowState { get; set; }

        /// <summary>
        /// The DataTable that contains the data to be copied
        /// </summary>
        public DataTable DataTable { get; set; }

        /// <summary>
        /// Name of the destination table on the server
        /// </summary>
        public string DestinationTableName { get; set; }

        /// <summary>
        /// The DbDataReader to stream the data to be copied
        /// </summary>
        public DbDataReader DbDataReader { get; set; }

        /// <summary>
        /// Enables or disables a <see cref="T:Microsoft.Data.SqlClient.SqlBulkCopy" /> object to stream data from an <see cref="T:System.Data.IDataReader" /> object
        /// </summary>
        public bool? EnableStreaming { get; set; }

        /// <summary>
        /// Specifies the type of map provided to ColumnMaps
        /// </summary>
        public MappingType MappingType { get; set; }

        /// <summary>
        /// Defines the number of rows to be processed before generating a notification event
        /// </summary>
        public int? NotifyAfter { get; set; }

        /// <summary>
        /// Delegate to be invoked every time that the number of rows specified by the NotifyAfter property have been processed.
        /// </summary>
        public Action<object, object> OnRowsCopied { get; set; }

        /// <summary>
        /// Options to be set for the command
        /// </summary>
        public BulkCopyCommandOptions Options { get; set; }
    }
}
