using FluentCommander.Core.Builders;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlNonQuery
{
    public class SqlNonQueryCommand : ParameterizedSqlCommandBuilder<SqlNonQueryCommand, SqlNonQueryResult>
    {
        private readonly IDatabaseRequestHandler _databaseRequestHandler;

        public SqlNonQueryCommand(IDatabaseRequestHandler databaseRequestHandler)
            : base(new SqlNonQueryRequest())
        {
            _databaseRequestHandler = databaseRequestHandler;
        }

        public override SqlNonQueryResult Execute()
        {
            return _databaseRequestHandler.ExecuteNonQuery((SqlNonQueryRequest)CommandRequest);
        }

        public override async Task<SqlNonQueryResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _databaseRequestHandler.ExecuteNonQueryAsync((SqlNonQueryRequest)CommandRequest, cancellationToken);
        }
    }
}