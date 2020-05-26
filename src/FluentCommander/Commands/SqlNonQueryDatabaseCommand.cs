using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Commands
{
    public class SqlNonQueryDatabaseCommand : ParameterizedDatabaseCommand<SqlNonQueryResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly SqlRequest _sqlRequest;

        public SqlNonQueryDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
            _sqlRequest = new SqlRequest();
        }

        public SqlNonQueryDatabaseCommand Sql(string sql)
        {
            _sqlRequest.Sql = sql;

            return this;
        }

        public override SqlNonQueryResult Execute()
        {
            _sqlRequest.DatabaseParameters = DatabaseParameters;
            _sqlRequest.Timeout = TimeoutTimeSpan;

            return _databaseCommander.ExecuteNonQuery(_sqlRequest);
        }

        public override async Task<SqlNonQueryResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            _sqlRequest.DatabaseParameters = DatabaseParameters;
            _sqlRequest.Timeout = TimeoutTimeSpan;

            return await _databaseCommander.ExecuteNonQueryAsync(_sqlRequest, cancellationToken);
        }
    }
}