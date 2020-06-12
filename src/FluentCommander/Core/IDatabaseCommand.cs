using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Core
{
    public interface IDatabaseCommand { }

    public interface IDatabaseCommand<TResult> : IDatabaseCommand
    {
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>The result of the command</returns>
        TResult Execute();

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
    }

    public interface IDatabaseCommand<TRequest, TResult> : IDatabaseCommand
    {
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        TResult Execute(TRequest request);

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
