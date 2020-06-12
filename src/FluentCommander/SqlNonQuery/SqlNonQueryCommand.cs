using FluentCommander.Core.CommandBuilders;
using FluentCommander.SqlQuery;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlNonQuery
{
    public class SqlNonQueryCommand : ParameterizedSqlCommandBuilder<SqlNonQueryCommand, SqlNonQueryResult>
    {
        private readonly IDatabaseCommander _databaseCommander;

        public SqlNonQueryCommand(IDatabaseCommander databaseCommander)
            : base(new SqlRequest())
        {
            _databaseCommander = databaseCommander;
        }

        public override SqlNonQueryResult Execute()
        {
            return _databaseCommander.ExecuteNonQuery(CommandRequest);
        }

        public override async Task<SqlNonQueryResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _databaseCommander.ExecuteNonQueryAsync(CommandRequest, cancellationToken);
        }
    }
}