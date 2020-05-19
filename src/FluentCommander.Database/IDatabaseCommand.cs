using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database
{
    public interface IDatabaseCommand { }

    public interface IDatabaseCommand<TResult> : IDatabaseCommand
    {
        TResult Execute();
        Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
