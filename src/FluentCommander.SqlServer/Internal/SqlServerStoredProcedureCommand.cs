using FluentCommander.Core;
using FluentCommander.Core.Behaviors;
using FluentCommander.StoredProcedure;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerStoredProcedureCommand : SqlServerCommandBase, IDatabaseCommand<StoredProcedureRequest, StoredProcedureResult>
    {
        private readonly ISqlServerConnectionProvider _connectionProvider;

        public SqlServerStoredProcedureCommand(ISqlServerConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public StoredProcedureResult Execute(StoredProcedureRequest request)
        {
            using SqlConnection connection = _connectionProvider.GetConnection();

            SqlCommand command = GetSqlCommand(request, connection);

            SqlParameter[] parameters = null;

            if (request.Parameters != null)
            {
                parameters = ToSqlParameters(request.Parameters);

                command.Parameters.AddRange(parameters);
            }

            var dataTable = new DataTable();

            new SqlDataAdapter(command).Fill(dataTable);

            HandleResult(request, parameters);

            return new StoredProcedureResult(dataTable, request.Parameters);
        }

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public async Task<StoredProcedureResult> ExecuteAsync(StoredProcedureRequest request, CancellationToken cancellationToken)
        {
            await using SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            SqlCommand command = GetSqlCommand(request, connection);

            SqlParameter[] parameters = null;

            if (request.Parameters != null)
            {
                parameters = ToSqlParameters(request.Parameters);

                command.Parameters.AddRange(parameters);
            }

            var dataTable = new DataTable();

            CommandBehavior behavior = ToSqlCommandBehaviors(request.ReadBehaviors);

            SqlDataReader reader = await command.ExecuteReaderAsync(behavior, cancellationToken);

            dataTable.Load(reader);

            HandleResult(request, parameters);

            return new StoredProcedureResult(dataTable, request.Parameters);
        }

        private SqlCommand GetSqlCommand(StoredProcedureRequest request, SqlConnection connection)
        {
            using var command = new SqlCommand(request.StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (request.Timeout.HasValue)
            {
                command.CommandTimeout = request.Timeout.Value.Seconds;
            }

            return command;
        }

        private void HandleResult(StoredProcedureRequest request, SqlParameter[] parameters)
        {
            if (request.Parameters == null || !request.Parameters.Any())
            {
                return;
            }

            HandleResultParameters(request, parameters, dp => dp.Direction == ParameterDirection.Output);
            HandleResultParameters(request, parameters, dp => dp.Direction == ParameterDirection.InputOutput);
            HandleResultParameters(request, parameters, dp => dp.Direction == ParameterDirection.ReturnValue);
        }

        private void HandleResultParameters(StoredProcedureRequest request, SqlParameter[] parameters, Func<DatabaseCommandParameter, bool> predicate)
        {
            foreach (DatabaseCommandParameter parameter in request.Parameters.Where(predicate))
            {
                parameter.Value = parameters.Single(sp => sp.ParameterName == parameter.Name).Value;
            }
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
