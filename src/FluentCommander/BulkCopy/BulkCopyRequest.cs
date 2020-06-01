﻿using FluentCommander.Core;
using System.Data;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyRequest : DatabaseCommandRequest
    {
        /// <summary>
        /// The name of the table to insert into
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// The dataTable that contains the data to be copied
        /// </summary>
        public DataTable DataTable { get; set; }

        /// <summary>
        /// Optional; maps the dataTable column names to the database table column names
        /// </summary>
        public ColumnMapping ColumnMapping { get; set; }
    }
}
