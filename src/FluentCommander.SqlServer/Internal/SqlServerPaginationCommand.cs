using FluentCommander.Core;
using FluentCommander.Pagination;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FluentCommander.UnitTests")]
namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerPaginationCommand : SqlServerCommandBase, IDatabaseCommand<PaginationRequest, PaginationResult>
    {
        private readonly IDatabaseRequestHandler _databaseRequestHandler;

        public SqlServerPaginationCommand(
            SqlConnectionStringBuilder builder,
            DatabaseCommandBuilder databaseCommandBuilder,
            ISqlServerCommandExecutor commandExecutor)
        {
            _databaseRequestHandler = new SqlServerDatabaseRequestHandler(builder, databaseCommandBuilder, commandExecutor);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public PaginationResult Execute(PaginationRequest request)
        {
            string sqlPagination = GetPaginationSql(request);
            string sqlPaginationCount = GetSqlRequestCount(request);

            DataTable dataTable = _databaseRequestHandler.ExecuteSql(sqlPagination);

            int totalCount = _databaseRequestHandler.ExecuteScalar<int>(sqlPaginationCount);

            return new PaginationResult(dataTable, totalCount);
        }

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public async Task<PaginationResult> ExecuteAsync(PaginationRequest request, CancellationToken cancellationToken)
        {
            string sqlPagination = GetPaginationSql(request);
            string sqlPaginationCount = GetSqlRequestCount(request);

            // These 2 database commands can be executed in parallel
            Task<DataTable> getDataTask = _databaseRequestHandler.ExecuteSqlAsync(sqlPagination, cancellationToken);
            Task<int> getCountTask = _databaseRequestHandler.ExecuteScalarAsync<int>(sqlPaginationCount, cancellationToken);

            await Task.WhenAll(getDataTask, getCountTask);

            // Get the results of the commands executed asynchronously
            DataTable dataTable = await getDataTask;
            int totalCount = await getCountTask;

            return new PaginationResult(dataTable, totalCount);
        }

        private string GetPaginationSql(PaginationRequest request)
        {
            string sql =
@"SELECT {0}
FROM {1} (NOLOCK)
WHERE 1 = 1 {2}
ORDER BY {3}
OFFSET {4} ROWS
FETCH NEXT {5} ROWS ONLY";

            request.SetDefaults();

            string offset = (request.PageSize * (request.PageNumber - 1)).ToString();
            string formattedSql = string.Format(sql, request.Columns, request.TableName, request.GetWhereClause(), request.OrderBy, offset, request.PageSize.ToString());

            return formattedSql;
        }

        private string GetSqlRequestCount(PaginationRequest request)
        {
            string sql =
@"SELECT COUNT(1)
FROM {0} (NOLOCK)
WHERE 1 = 1 {1}";

            request.SetDefaults();

            string formattedSql = string.Format(sql, request.TableName, request.GetWhereClause());

            return formattedSql;
        }
    }
}
