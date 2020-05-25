﻿using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public class SqlQueryDatabaseCommand : ParameterizedDatabaseCommand<SqlQueryResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly SqlRequest _sqlRequest;

        public SqlQueryDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
            _sqlRequest = new SqlRequest();
        }

        public SqlQueryDatabaseCommand Sql(string sql)
        {
            _sqlRequest.Sql = sql;

            return this;
        }

        public SqlQueryDatabaseCommand Timeout(int timeoutInSeconds)
        {
            _sqlRequest.TimeoutInSeconds = timeoutInSeconds;

            return this;
        }

        public override SqlQueryResult Execute()
        {
            _sqlRequest.DatabaseParameters = DatabaseParameters;

            return _databaseCommander.ExecuteSql(_sqlRequest);
        }

        public override async Task<SqlQueryResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            _sqlRequest.DatabaseParameters = DatabaseParameters;

            return await _databaseCommander.ExecuteSqlAsync(_sqlRequest, cancellationToken);
        }
    }
}