using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerSqlNonQueryCommand : SqlServerSqlCommand<SqlNonQueryResult>
    {
        private readonly ISqlServerConnectionProvider _connectionProvider;

        public SqlServerSqlNonQueryCommand(ISqlServerConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public override SqlNonQueryResult Execute(SqlRequest request)
        {
            using SqlConnection connection = _connectionProvider.GetConnection();

            using SqlCommand command = GetSqlCommand(connection, request);

            int numberOfRowsAffected = command.ExecuteNonQuery();

            return new SqlNonQueryResult(numberOfRowsAffected);
        }

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public override async Task<SqlNonQueryResult> ExecuteAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            await using SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = GetSqlCommand(connection, request);

            int numberOfRowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);

            return new SqlNonQueryResult(numberOfRowsAffected);
        }
    }
}
