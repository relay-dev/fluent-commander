using FluentCommander.Core.CommandBuilders;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlQuery
{
    public class SqlQueryCommand : ParameterizedSqlCommandBuilder<SqlRequest, SqlQueryCommand, SqlQueryResult>
    {
        private readonly IDatabaseCommander _databaseCommander;

        public SqlQueryCommand(IDatabaseCommander databaseCommander)
            : base(new SqlRequest())
        {
            _databaseCommander = databaseCommander;
        }

        public override SqlQueryResult Execute()
        {
            return _databaseCommander.ExecuteSql(CommandRequest);
        }

        public override async Task<SqlQueryResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _databaseCommander.ExecuteSqlAsync(CommandRequest, cancellationToken);
        }
    }
}