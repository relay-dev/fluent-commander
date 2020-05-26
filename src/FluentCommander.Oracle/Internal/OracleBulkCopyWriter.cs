﻿using System;
using System.Data;

namespace FluentCommander.Oracle.Internal
{
    public class OracleBulkCopyWriter
    {
        private readonly BulkCopyRequest _bulkCopyRequest;

        public OracleBulkCopyWriter(BulkCopyRequest bulkCopyRequest)
        {
            _bulkCopyRequest = bulkCopyRequest;
        }

        public SqlRequest ToSqlRequest(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
    }
}