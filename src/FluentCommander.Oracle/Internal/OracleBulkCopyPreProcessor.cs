using FluentCommander.BulkCopy;
using FluentCommander.SqlNonQuery;
using System;
using System.Data;

namespace FluentCommander.Oracle.Internal
{
    internal class OracleBulkCopyPreProcessor
    {
        private readonly BulkCopyRequest _bulkCopyRequest;

        public OracleBulkCopyPreProcessor(BulkCopyRequest bulkCopyRequest)
        {
            _bulkCopyRequest = bulkCopyRequest;
        }

        public SqlNonQueryRequest ToSqlRequest(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
    }
}
