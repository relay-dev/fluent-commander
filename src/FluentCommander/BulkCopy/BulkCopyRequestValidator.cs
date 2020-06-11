using System;
using FluentCommander.Core;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyRequestValidator : IRequestValidator<BulkCopyRequest>
    {
        public void Validate(BulkCopyRequest request)
        {
            if (string.IsNullOrEmpty(request.DestinationTableName))
            {
                throw new InvalidOperationException("Into(tableName) must be called before calling the Execute() method");
            }

            if (request.EnableStreaming.GetValueOrDefault() && request.DataReader == null && request.DbDataReader == null)
            {
                throw new InvalidOperationException("DataReader cannot be null");
            }

            if (request.DataTable == null && request.DataRows == null)
            {
                throw new InvalidOperationException("From(dataTable) must be called before calling the Execute() method");
            }
        }
    }
}
