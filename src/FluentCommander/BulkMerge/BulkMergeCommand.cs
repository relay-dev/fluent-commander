using FluentCommander.BulkCopy;
using FluentCommander.Core;
using FluentCommander.Core.Utility;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkMerge
{
    public class BulkMergeCommand : BulkCopyCommand
    {
        private readonly IDatabaseCommander _databaseCommander;

        public BulkMergeCommand(
            IDatabaseCommander databaseCommander,
            IRequestValidator<BulkCopyRequest> requestValidator,
            IAutoMapper autoMapper)
            : base(databaseCommander, requestValidator, autoMapper)
        {
            _databaseCommander = databaseCommander;
        }

        /// <summary>Executes the command</summary>
        /// <returns>The result of the command</returns>
        public override BulkCopyResult Execute()
        {
            CommandRequest.DestinationTableName = $"#{CommandRequest.DestinationTableName}";

            BulkCopyResult result = base.Execute();

            // TODO: Implement merge command

            return result;
        }

        /// <summary>Executes the command asynchronously</summary>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public override async Task<BulkCopyResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            CommandRequest.DestinationTableName = $"#{CommandRequest.DestinationTableName}";

            BulkCopyResult result = await base.ExecuteAsync(cancellationToken);

            // TODO: Implement merge command

            return result;
        }
    }
}
