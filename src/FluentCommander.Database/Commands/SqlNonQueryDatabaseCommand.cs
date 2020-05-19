using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public class SqlNonQueryDatabaseCommand : ParameterizedDatabaseCommand<SqlNonQueryCommandResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private string _sql;

        public SqlNonQueryDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public SqlNonQueryDatabaseCommand Sql(string sql)
        {
            _sql = sql;

            return this;
        }

        public override SqlNonQueryCommandResult Execute()
        {
            int rowCountAffected = _databaseCommander.ExecuteNonQuery(_sql, Parameters);

            return new SqlNonQueryCommandResult(rowCountAffected);
        }

        public override async Task<SqlNonQueryCommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            int rowCountAffected = await _databaseCommander.ExecuteNonQueryAsync(_sql, cancellationToken, Parameters);

            return new SqlNonQueryCommandResult(rowCountAffected);
        }
    }
}