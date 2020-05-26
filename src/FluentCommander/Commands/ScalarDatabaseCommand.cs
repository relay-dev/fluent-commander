﻿using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Commands
{
    public class ScalarDatabaseCommand<TResult> : ParameterizedDatabaseCommand<TResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly SqlRequest _sqlRequest;

        public ScalarDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
            _sqlRequest = new SqlRequest();
        }

        public ScalarDatabaseCommand<TResult> Sql(string sql)
        {
            _sqlRequest.Sql = sql;

            return this;
        }

        public override TResult Execute()
        {
            _sqlRequest.DatabaseParameters = DatabaseParameters;
            _sqlRequest.Timeout = TimeoutTimeSpan;

            return _databaseCommander.ExecuteScalar<TResult>(_sqlRequest);
        }

        public override async Task<TResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            _sqlRequest.DatabaseParameters = DatabaseParameters;
            _sqlRequest.Timeout = TimeoutTimeSpan;

            return await _databaseCommander.ExecuteScalarAsync<TResult>(_sqlRequest, cancellationToken);
        }
    }
}