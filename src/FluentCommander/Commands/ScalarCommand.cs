using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Commands
{
    public class ScalarCommand<TResult> : ParameterizedSqlCommand<ScalarCommand<TResult>, TResult>
    {
        private readonly IDatabaseCommander _databaseCommander;

        public ScalarCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public override TResult Execute()
        {
            SqlRequest.DatabaseParameters = Parameters;
            SqlRequest.Timeout = CommandTimeout;

            return _databaseCommander.ExecuteScalar<TResult>(SqlRequest);
        }

        public override async Task<TResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            SqlRequest.DatabaseParameters = Parameters;
            SqlRequest.Timeout = CommandTimeout;

            return await _databaseCommander.ExecuteScalarAsync<TResult>(SqlRequest, cancellationToken);
        }
    }
}