using FluentCommander.Core.CommandBuilders;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkCopy
{
    public abstract partial class BulkCopyCommandBuilder : CommandBuilderBase<BulkCopyRequest, BulkCopyCommandBuilder, BulkCopyResult>
    {
        private readonly BulkCopyMappingOptions _options;
        protected bool IsAutoMap;

        protected BulkCopyCommandBuilder()
        {
            CommandRequest = new BulkCopyRequest();
            _options = new BulkCopyMappingOptions(this);
        }

        public BulkCopyCommandBuilder From(DataTable dataTable)
        {
            CommandRequest.DataTable = dataTable;

            return this;
        }

        public BulkCopyCommandBuilder Into(string tableName)
        {
            CommandRequest.TableName = tableName;

            return this;
        }

        public BulkCopyCommandBuilder MappingOptions(Func<BulkCopyMappingOptions, BulkCopyMappingOptions> options)
        {
            options.Invoke(_options);

            return this;
        }

        public abstract override BulkCopyResult Execute();
        public abstract override Task<BulkCopyResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
