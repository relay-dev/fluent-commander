using FluentCommander.Core;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer.Internal
{
    internal abstract class SqlServerSqlCommand<TResult> : SqlServerCommandBase, IDatabaseCommand<SqlRequest, TResult>
    {
        protected SqlCommand GetSqlCommand(SqlConnection connection, SqlRequest request)
        {
            using var command = new SqlCommand(request.Sql, connection);

            if (request.Parameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(request.Parameters));
            }

            if (request.Timeout.HasValue)
            {
                command.CommandTimeout = request.Timeout.Value.Seconds;
            }

            return command;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public abstract TResult Execute(SqlRequest request);

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public abstract Task<TResult> ExecuteAsync(SqlRequest request, CancellationToken cancellationToken);
    }
}
