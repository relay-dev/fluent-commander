using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Core.CommandBuilders;

namespace FluentCommander.SqlQuery
{
    public class SqlQueryCommand : ParameterizedSqlCommand<SqlQueryCommand, SqlQueryResult>
    {
        private readonly IDatabaseCommander _databaseCommander;

        public SqlQueryCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public override SqlQueryResult Execute()
        {
            SqlRequest.DatabaseParameters = Parameters;
            SqlRequest.Timeout = CommandTimeout;

            return _databaseCommander.ExecuteSql(SqlRequest);
        }

        public override async Task<SqlQueryResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            SqlRequest.DatabaseParameters = Parameters;
            SqlRequest.Timeout = CommandTimeout;

            return await _databaseCommander.ExecuteSqlAsync(SqlRequest, cancellationToken);
        }
    }
}