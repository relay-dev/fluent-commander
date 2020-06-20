using FluentCommander.BulkCopy;
using FluentCommander.Core;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

#if DEBUG
[assembly: InternalsVisibleTo("FluentCommander.UnitTests")]
#endif
namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerBulkCopyCommand : SqlServerCommandBase, IDatabaseCommand<BulkCopyRequest, BulkCopyResult>
    {
        private readonly ISqlServerConnectionProvider _connectionProvider;
        public SqlServerBulkCopyCommand(ISqlServerConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public BulkCopyResult Execute(BulkCopyRequest request)
        {
            using SqlConnection connection = _connectionProvider.GetConnection();

            SqlBulkCopy sqlBulkCopy = GetSqlBulkCopy(connection, request);

            Action bulkCopy = ResolveBulkCopyOverload(request, sqlBulkCopy);

            try
            {
                bulkCopy();
            }
            catch (Exception e)
            {
                HandleBulkCopyException(e, sqlBulkCopy);

                throw;
            }

            return new BulkCopyResult(request.DataTable.Rows.Count);
        }

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public async Task<BulkCopyResult> ExecuteAsync(BulkCopyRequest request, CancellationToken cancellationToken)
        {
            await using SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            SqlBulkCopy sqlBulkCopy = GetSqlBulkCopy(connection, request);

            Func<Task> bulkCopy = ResolveBulkCopyAsyncOverload(request, sqlBulkCopy, cancellationToken);

            try
            {
                await bulkCopy();
            }
            catch (Exception e)
            {
                HandleBulkCopyException(e, sqlBulkCopy);

                throw;
            }

            return new BulkCopyResult(request.DataTable.Rows.Count);
        }

        private SqlBulkCopy GetSqlBulkCopy(SqlConnection connection, BulkCopyRequest request)
        {
            var command = request.Options == null
                ? new SqlBulkCopy(connection)
                : new SqlBulkCopy(connection.ConnectionString, ToSqlBulkCopyOptions(request));

            command.DestinationTableName = request.DestinationTableName;
            
            if (request.BatchSize.HasValue)
            {
                command.BatchSize = request.BatchSize.Value;
            }

            if (request.EnableStreaming.HasValue)
            {
                command.EnableStreaming = request.EnableStreaming.Value;
            }

            if (request.NotifyAfter.HasValue)
            {
                command.NotifyAfter = request.NotifyAfter.Value;
            }

            if (request.Timeout.HasValue)
            {
                command.BulkCopyTimeout = request.Timeout.Value.Seconds;
            }

            if (request.ColumnMapping != null)
            {
                foreach (ColumnMap columnMap in request.ColumnMapping.ColumnMaps)
                {
                    command.ColumnMappings.Add(columnMap.Source, columnMap.Destination);
                }
            }

            if (request.OnRowsCopied != null)
            {
                command.SqlRowsCopied += (sender, e) =>
                {
                    if (e.Abort)
                    {
                        throw new BulkCopyException("The BulkCopy command was aborted", e.RowsCopied);
                    }

                    request.OnRowsCopied(sender, e);
                };
            }

            return command;
        }

        private Action ResolveBulkCopyOverload(BulkCopyRequest request, SqlBulkCopy sqlBulkCopy)
        {
            Action bulkCopy;

            if (request.EnableStreaming.GetValueOrDefault())
            {
                if (request.DbDataReader != null)
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServer(request.DataReader);
                }
                else
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServer(request.DbDataReader);
                }
            }
            else
            {
                if (request.DataRowState.HasValue)
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServer(request.DataTable, request.DataRowState.Value);
                }
                else if (request.DataRows != null && request.DataRows.Any())
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServer(request.DataRows);
                }
                else
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServer(request.DataTable);
                }
            }

            return bulkCopy;
        }

        private Func<Task> ResolveBulkCopyAsyncOverload(BulkCopyRequest request, SqlBulkCopy sqlBulkCopy, CancellationToken cancellationToken)
        {
            Func<Task> bulkCopy;

            if (request.EnableStreaming.GetValueOrDefault())
            {
                if (request.DbDataReader != null)
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServerAsync(request.DataReader, cancellationToken);
                }
                else
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServerAsync(request.DbDataReader, cancellationToken);
                }
            }
            else
            {
                if (request.DataRowState.HasValue)
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServerAsync(request.DataTable, request.DataRowState.Value, cancellationToken);
                }
                else if (request.DataRows != null && request.DataRows.Any())
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServerAsync(request.DataRows, cancellationToken);
                }
                else
                {
                    bulkCopy = () => sqlBulkCopy.WriteToServerAsync(request.DataTable, cancellationToken);
                }
            }

            return bulkCopy;
        }

        private SqlBulkCopyOptions ToSqlBulkCopyOptions(BulkCopyRequest request)
        {
            SqlBulkCopyOptions options = SqlBulkCopyOptions.Default;

            if (request.Options != null)
            {
                options = SetFlag(options, SqlBulkCopyOptions.KeepIdentity, request.Options.KeepIdentity);
                options = SetFlag(options, SqlBulkCopyOptions.CheckConstraints, request.Options.CheckConstraints);
                options = SetFlag(options, SqlBulkCopyOptions.TableLock, request.Options.TableLock);
                options = SetFlag(options, SqlBulkCopyOptions.KeepNulls, request.Options.KeepNulls);
                options = SetFlag(options, SqlBulkCopyOptions.FireTriggers, request.Options.FireTriggers);
                options = SetFlag(options, SqlBulkCopyOptions.UseInternalTransaction, request.Options.UseInternalTransaction);
                options = SetFlag(options, SqlBulkCopyOptions.AllowEncryptedValueModifications, request.Options.AllowEncryptedValueModifications);
            }

            return options;
        }

        private void HandleBulkCopyException(Exception e, SqlBulkCopy sqlBulkCopy)
        {
            if (!e.Message.Contains("Received an invalid column length from the bcp client for colid"))
            {
                return;
            }

            try
            {
                string pattern = @"\d+";
                Match match = Regex.Match(e.Message, pattern);
                var index = Convert.ToInt32(match.Value) - 1;

                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi == null) return;
                var sortedColumns = fi.GetValue(sqlBulkCopy);
                var items = (object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(sortedColumns);

                if (items == null) return;
                FieldInfo itemData = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                if (itemData == null) return;
                var metadata = itemData.GetValue(items[index]);

                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(metadata);
                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(metadata);

                throw new BulkCopyException($"Column '{column}' contains a string whose length is greater than {length} characters", e);
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
