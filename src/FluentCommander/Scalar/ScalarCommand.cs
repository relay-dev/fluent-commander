using FluentCommander.Core.CommandBuilders;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Scalar
{
    public class ScalarCommand<TResult> : ParameterizedSqlCommandBuilder<ScalarCommand<TResult>, ScalarCommand<TResult>, TResult>
    {
        private readonly IDatabaseCommander _databaseCommander;

        public ScalarCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public override TResult Execute()
        {
            SqlRequest.Parameters = Parameters;
            SqlRequest.Timeout = CommandTimeout;

            return _databaseCommander.ExecuteScalar<TResult>(SqlRequest);
        }

        public override async Task<TResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            SqlRequest.Parameters = Parameters;
            SqlRequest.Timeout = CommandTimeout;

            return await _databaseCommander.ExecuteScalarAsync<TResult>(SqlRequest, cancellationToken);
        }
    }
}