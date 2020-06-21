using FluentCommander.Core;
using FluentCommander.Core.Behaviors;
using FluentCommander.SqlQuery;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FluentCommander.UnitTests")]
namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerSqlQueryCommand : SqlServerSqlCommand<SqlQueryResult>
    {
        private readonly ISqlServerConnectionProvider _connectionProvider;
        private readonly ISqlServerCommandExecutor _commandExecutor;

        public SqlServerSqlQueryCommand(
            ISqlServerConnectionProvider connectionProvider,
            ISqlServerCommandExecutor commandExecutor)
        {
            _connectionProvider = connectionProvider;
            _commandExecutor = commandExecutor;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public override SqlQueryResult Execute(SqlRequest request)
        {
            using SqlConnection connection = _connectionProvider.GetConnection(request.Options);

            using SqlCommand command = GetSqlCommand(connection, request);

            DataTable dataTable = _commandExecutor.Execute(command);

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

            CommandBehavior behavior = ToSqlCommandBehaviors(((SqlQueryRequest)request).ReadBehaviors);

            var reader = await command.ExecuteReaderAsync(behavior, cancellationToken);

            dataTable.Load(reader);

            return new SqlQueryResult(dataTable);
        }

        private CommandBehavior ToSqlCommandBehaviors(ReadBehaviors behaviors)
        {
            CommandBehavior behavior = CommandBehavior.Default;

            if (behaviors != null)
            {
                behavior = SetFlag(behavior, CommandBehavior.SingleResult, behaviors.SingleResult);
                behavior = SetFlag(behavior, CommandBehavior.SchemaOnly, behaviors.SchemaOnly);
                behavior = SetFlag(behavior, CommandBehavior.KeyInfo, behaviors.KeyInfo);
                behavior = SetFlag(behavior, CommandBehavior.SingleRow, behaviors.SingleRow);
                behavior = SetFlag(behavior, CommandBehavior.SequentialAccess, behaviors.SequentialAccess);
                behavior = SetFlag(behavior, CommandBehavior.CloseConnection, behaviors.CloseConnection);
            }

            return behavior;
        }
    }
}
