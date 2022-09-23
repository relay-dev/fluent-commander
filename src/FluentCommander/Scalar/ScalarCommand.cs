using FluentCommander.Core;
using FluentCommander.Core.Builders;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Scalar
{
    public class ScalarCommand<TResult> : ParameterizedSqlCommandBuilder<ScalarCommand<TResult>, TResult>
    {
        private readonly IDatabaseRequestHandler _databaseRequestHandler;

        public ScalarCommand(IDatabaseRequestHandler databaseRequestHandler)
            : base(new ScalarRequest())
        {
            _databaseRequestHandler = databaseRequestHandler;
        }

        public override TResult Execute()
        {
            return _databaseRequestHandler.ExecuteScalar<TResult>((ScalarRequest)CommandRequest);
        }

        public override async Task<TResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _databaseRequestHandler.ExecuteScalarAsync<TResult>((ScalarRequest)CommandRequest, cancellationToken);
        }
    }
}