using FluentCommander.BulkCopy;
using FluentCommander.Core;
using FluentCommander.Pagination;
using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
using FluentCommander.SqlServer.Internal;
using FluentCommander.StoredProcedure;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer
{
    public class SqlServerDatabaseCommander : DatabaseCommanderBase
    {
        private readonly IDatabaseCommandFactory _commandFactory;

        public SqlServerDatabaseCommander(
            IDatabaseCommandFactory commandFactory,
            DatabaseCommandBuilder databaseCommandBuilder)
            : base(databaseCommandBuilder)
        {
            _commandFactory = commandFactory;
        }

        public override BulkCopyResult BulkCopy(BulkCopyRequest request)
        {
            return _commandFactory.Create<SqlServerBulkCopyCommand>().Execute(request);
        }

        public override async Task<BulkCopyResult> BulkCopyAsync(BulkCopyRequest request, CancellationToken cancellationToken)
        {
            return await _commandFactory.Create<SqlServerBulkCopyCommand>().ExecuteAsync(request, cancellationToken);
        }

        public override SqlNonQueryResult ExecuteNonQuery(SqlRequest request)
        {
            return _commandFactory.Create<SqlServerSqlNonQueryCommand>().Execute(request);
        }

        public override async Task<SqlNonQueryResult> ExecuteNonQueryAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            return await _commandFactory.Create<SqlServerSqlNonQueryCommand>().ExecuteAsync(request, cancellationToken);
        }

        public override int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(new SqlRequest(sql)).RowCountAffected;
        }

        public override async Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken)
        {
            return (await ExecuteNonQueryAsync(new SqlRequest(sql), cancellationToken)).RowCountAffected;
        }

        public override TResult ExecuteScalar<TResult>(SqlRequest request)
        {
            return _commandFactory.Create<SqlServerScalarCommand<TResult>>().Execute(request);
        }

        public override async Task<TResult> ExecuteScalarAsync<TResult>(SqlRequest request, CancellationToken cancellationToken)
        {
            return await _commandFactory.Create<SqlServerScalarCommand<TResult>>().ExecuteAsync(request, cancellationToken);
        }

        public override TResult ExecuteScalar<TResult>(string sql)
        {
            return ExecuteScalar<TResult>(new SqlRequest(sql));
        }

        public override async Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken)
        {
            return await ExecuteScalarAsync<TResult>(new SqlRequest(sql), cancellationToken);
        }

        public override SqlQueryResult ExecuteSql(SqlRequest request)
        {
            return _commandFactory.Create<SqlServerSqlQueryCommand>().Execute(request);
        }

        public override async Task<SqlQueryResult> ExecuteSqlAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            return await _commandFactory.Create<SqlServerSqlQueryCommand>().ExecuteAsync(request, cancellationToken);
        }

        public override DataTable ExecuteSql(string sql)
        {
            return ExecuteSql(new SqlRequest(sql)).DataTable;
        }

        public override async Task<DataTable> ExecuteSqlAsync(string sql, CancellationToken cancellationToken)
        {
            return (await ExecuteSqlAsync(new SqlRequest(sql), cancellationToken)).DataTable;
        }

        public override StoredProcedureResult ExecuteStoredProcedure(StoredProcedureRequest request)
        {
            return _commandFactory.Create<SqlServerStoredProcedureCommand>().Execute(request);
        }

        public override async Task<StoredProcedureResult> ExecuteStoredProcedureAsync(StoredProcedureRequest request, CancellationToken cancellationToken)
        {
            return await _commandFactory.Create<SqlServerStoredProcedureCommand>().ExecuteAsync(request, cancellationToken);
        }

        public override string GetServerName()
        {
            return ExecuteScalar<string>(SqlSelectServerName);
        }

        public override async Task<string> GetServerNameAsync(CancellationToken cancellationToken)
        {
            return await ExecuteScalarAsync<string>(SqlSelectServerName, cancellationToken);
        }

        public override PaginationResult Paginate(PaginationRequest request)
        {
            return _commandFactory.Create<SqlServerPaginationCommand>().Execute(request);
        }

        public override async Task<PaginationResult> PaginateAsync(PaginationRequest request, CancellationToken cancellationToken)
        {
            return await _commandFactory.Create<SqlServerPaginationCommand>().ExecuteAsync(request, cancellationToken);
        }

        private string SqlSelectServerName => "SELECT @@SERVERNAME";
    }
}
