using System;
using System.Data;
using FluentCommander.BulkCopy;
using FluentCommander.SqlQuery;

namespace FluentCommander.Oracle.Internal
{
    internal class OracleBulkCopyPreProcessor
    {
        private readonly BulkCopyRequest _bulkCopyRequest;

        public OracleBulkCopyPreProcessor(BulkCopyRequest bulkCopyRequest)
        {
            _bulkCopyRequest = bulkCopyRequest;
        }

        public SqlRequest ToSqlRequest(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
    }
}
