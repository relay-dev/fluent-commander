using FluentCommander.BulkCopy;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkMerge
{
    public abstract class BulkMergeCommandBuilder : BulkCopyCommandBuilder
    {
        protected BulkMergeCommandBuilder(BulkCopyRequest commandRequest)
            : base(commandRequest) { }

        public abstract override BulkCopyResult Execute();
        public abstract override Task<BulkCopyResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
