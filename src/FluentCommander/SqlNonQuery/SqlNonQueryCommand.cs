using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Core.CommandBuilders;

namespace FluentCommander.SqlNonQuery
{
    public class SqlNonQueryCommand : ParameterizedSqlCommand<SqlNonQueryCommand, SqlNonQueryResult>
    {
        private readonly IDatabaseCommander _databaseCommander;

        public SqlNonQueryCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public override SqlNonQueryResult Execute()
        {
            SqlRequest.DatabaseParameters = Parameters;
            SqlRequest.Timeout = CommandTimeout;

            return _databaseCommander.ExecuteNonQuery(SqlRequest);
        }

        public override async Task<SqlNonQueryResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            SqlRequest.DatabaseParameters = Parameters;
            SqlRequest.Timeout = CommandTimeout;

            return await _databaseCommander.ExecuteNonQueryAsync(SqlRequest, cancellationToken);
        }
    }
}