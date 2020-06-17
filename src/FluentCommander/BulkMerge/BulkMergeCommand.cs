using FluentCommander.BulkCopy;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkMerge
{
    public class BulkMergeCommand : BulkMergeCommandBuilder
    {
        public BulkMergeCommand(BulkCopyRequest commandRequest) : base(commandRequest)
        {
        }

        /// <summary>Executes the command</summary>
        /// <returns>The result of the command</returns>
        public override BulkCopyResult Execute()
        {
            return null;
        }

        /// <summary>Executes the command asynchronously</summary>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public override async Task<BulkCopyResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
