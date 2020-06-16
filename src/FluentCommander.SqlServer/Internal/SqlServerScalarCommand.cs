using FluentCommander.Core;
using Microsoft.Data.SqlClient;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FluentCommander.UnitTests")]
namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerScalarCommand<TResult> : SqlServerSqlCommand<TResult>
    {
        private readonly ISqlServerConnectionProvider _connectionProvider;

        public SqlServerScalarCommand(ISqlServerConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public override TResult Execute(SqlRequest request)
        {
            using SqlConnection connection = _connectionProvider.GetConnection();

            using SqlCommand command = GetSqlCommand(connection, request);

            object result = command.ExecuteScalar();

            return result == null || result == DBNull.Value
                ? default
                : (TResult)result;
        }

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public override async Task<TResult> ExecuteAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            await using SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = GetSqlCommand(connection, request);

            object result = await command.ExecuteScalarAsync(cancellationToken);

            return result == null || result == DBNull.Value
                ? default
                : (TResult)result;
        }
    }
}
