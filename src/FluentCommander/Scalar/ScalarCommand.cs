using FluentCommander.Core.CommandBuilders;
using FluentCommander.SqlQuery;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Scalar
{
    public class ScalarCommand<TResult> : ParameterizedSqlCommandBuilder<ScalarCommand<TResult>, TResult>
    {
        private readonly IDatabaseCommander _databaseCommander;

        public ScalarCommand(IDatabaseCommander databaseCommander)
            : base(new SqlRequest())
        {
            _databaseCommander = databaseCommander;
        }

        public override TResult Execute()
        {
            return _databaseCommander.ExecuteScalar<TResult>(CommandRequest);
        }

        public override async Task<TResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _databaseCommander.ExecuteScalarAsync<TResult>(CommandRequest, cancellationToken);
        }
    }
}