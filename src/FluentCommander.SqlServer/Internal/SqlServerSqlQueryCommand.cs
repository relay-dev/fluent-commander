using FluentCommander.SqlQuery;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerSqlQueryCommand : SqlServerSqlCommand<SqlQueryResult>
    {
        private readonly ISqlServerConnectionProvider _connectionProvider;

        public SqlServerSqlQueryCommand(ISqlServerConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public override SqlQueryResult Execute(SqlRequest request)
        {
            using SqlConnection connection = _connectionProvider.GetConnection();

            using SqlCommand command = GetSqlCommand(connection, request);

            var dataTable = new DataTable();
            
            new SqlDataAdapter(command).Fill(dataTable);

            return new SqlQueryResult(dataTable);
        }

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public override async Task<SqlQueryResult> ExecuteAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            await using SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = GetSqlCommand(connection, request);

            var dataTable = new DataTable();

            var reader = await command.ExecuteReaderAsync(cancellationToken);

            dataTable.Load(reader);

            return new SqlQueryResult(dataTable);
        }
    }
}
