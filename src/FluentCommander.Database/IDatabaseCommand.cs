using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database
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
}
