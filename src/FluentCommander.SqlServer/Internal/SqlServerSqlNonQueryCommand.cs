using FluentCommander.Core;
using FluentCommander.SqlNonQuery;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FluentCommander.UnitTests")]
namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerSqlNonQueryCommand : SqlServerSqlCommand<SqlNonQueryResult>
    {
        public SqlServerSqlNonQueryCommand(ISqlServerConnectionProvider connectionProvider)
            : base(connectionProvider) { }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public override SqlNonQueryResult Execute(SqlRequest request)
        {
            using SqlConnection connection = GetSqlConnection(request);

            using SqlCommand command = GetSqlCommand(request, connection);

            int result = command.ExecuteNonQuery();

            return new SqlNonQueryResult(result);
        }

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public override async Task<SqlNonQueryResult> ExecuteAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            await using SqlConnection connection = await GetSqlConnectionAsync(request, cancellationToken);

            await using SqlCommand command = GetSqlCommand(request, connection);

            int result = await command.ExecuteNonQueryAsync(cancellationToken);

            return new SqlNonQueryResult(result);
        }
    }
}
