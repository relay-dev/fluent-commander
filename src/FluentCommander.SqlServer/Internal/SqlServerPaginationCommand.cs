using FluentCommander.Core;
using FluentCommander.Pagination;
using FluentCommander.SqlQuery;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerPaginationCommand : SqlServerCommand, IDatabaseCommand<PaginationRequest, PaginationResult>
    {
        private readonly IDatabaseCommandFactory _databaseCommandFactory;

        public SqlServerPaginationCommand(IDatabaseCommandFactory databaseCommandFactory)
        {
            _databaseCommandFactory = databaseCommandFactory;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <returns>The result of the command</returns>
        public PaginationResult Execute(PaginationRequest request)
        {
            SqlRequest sqlRequest = GetSqlRequest(request);
            SqlRequest sqlRequestCount = GetSqlRequestCount(request);

            SqlQueryResult result = _databaseCommandFactory.Create<SqlServerSqlQueryCommand>().Execute(sqlRequest);

            int totalCount = _databaseCommandFactory.Create<SqlServerScalarCommand<int>>().Execute(sqlRequestCount);

            return new PaginationResult(result.DataTable, totalCount);
        }

        /// <summary>
        /// Executes the command asynchronously
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public async Task<PaginationResult> ExecuteAsync(PaginationRequest request, CancellationToken cancellationToken)
        {
            SqlRequest sqlRequest = GetSqlRequest(request);
            SqlRequest sqlRequestCount = GetSqlRequestCount(request);

            // These 2 database commands can be executed in parallel
            Task<SqlQueryResult> getDataTask = _databaseCommandFactory.Create<SqlServerSqlQueryCommand>().ExecuteAsync(sqlRequest, cancellationToken);
            Task<int> getCountTask = _databaseCommandFactory.Create<SqlServerScalarCommand<int>>().ExecuteAsync(sqlRequestCount, cancellationToken);

            await Task.WhenAll(getDataTask, getCountTask);

            // Get the results of the commands executed asynchronously
            SqlQueryResult result = await getDataTask;
            int totalCount = await getCountTask;

            return new PaginationResult(result.DataTable, totalCount);
        }

        private SqlRequest GetSqlRequest(PaginationRequest request)
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

            return new SqlRequest(formattedSql);
        }

        private SqlRequest GetSqlRequestCount(PaginationRequest request)
        {
            string sql =
@"SELECT COUNT(1)
FROM {0} (NOLOCK)
WHERE 1 = 1 {1}";

            request.SetDefaults();

            string formattedSql = string.Format(sql, request.TableName, request.GetWhereClause());

            return new SqlRequest(formattedSql);
        }
    }
}
