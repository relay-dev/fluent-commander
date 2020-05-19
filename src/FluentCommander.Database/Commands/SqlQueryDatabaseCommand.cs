using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public class SqlQueryDatabaseCommand : ParameterizedDatabaseCommand<SqlQueryCommandResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private string _sql;

        public SqlQueryDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public SqlQueryDatabaseCommand Sql(string sql)
        {
            _sql = sql;

            return this;
        }

        public override SqlQueryCommandResult Execute()
        {
            DataTable dataTable = _databaseCommander.ExecuteSql(_sql, Parameters);

            return new SqlQueryCommandResult(Parameters, dataTable);
        }

        public override async Task<SqlQueryCommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            DataTable dataTable = await _databaseCommander.ExecuteSqlAsync(_sql, cancellationToken, Parameters);

            return new SqlQueryCommandResult(Parameters, dataTable);
        }
    }
}