using FluentCommander.Core.CommandBuilders;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkCopy
{
    public abstract class BulkCopyCommandBuilder : CommandBuilder<BulkCopyRequest, BulkCopyCommandBuilder, BulkCopyResult>
    {
        internal readonly BulkCopyRequest CommandRequest;
        internal readonly BulkCopyMappingOptions MappingOptions;

        protected BulkCopyCommandBuilder(BulkCopyRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
            MappingOptions = new BulkCopyMappingOptions(commandRequest);
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

        public BulkCopyCommandBuilder Mapping(Func<BulkCopyMappingOptions, BulkCopyMappingOptions> options)
        {
            options.Invoke(MappingOptions);

            return this;
        }

        public abstract override BulkCopyResult Execute();
        public abstract override Task<BulkCopyResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
