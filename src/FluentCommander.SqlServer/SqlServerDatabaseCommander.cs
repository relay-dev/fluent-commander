using FluentCommander.BulkCopy;
using FluentCommander.Pagination;
using FluentCommander.Scalar;
using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
using FluentCommander.SqlServer.Internal;
using FluentCommander.StoredProcedure;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer
{
    public class SqlServerDatabaseCommander : DatabaseCommanderBase
    {
        private readonly SqlConnectionStringBuilder _builder;
        private readonly ISqlServerCommandExecutor _commandExecutor;

        public SqlServerDatabaseCommander(
            SqlConnectionStringBuilder builder,
            DatabaseCommandBuilder databaseCommandBuilder,
            ISqlServerCommandExecutor commandExecutor)
            : base(databaseCommandBuilder)
        {
            _builder = builder;
            _commandExecutor = commandExecutor;
        }

        public override BulkCopyResult BulkCopy(BulkCopyRequest request)
        {
            return new SqlServerBulkCopyCommand(ConnectionProvider).Execute(request);
        }

        public override async Task<BulkCopyResult> BulkCopyAsync(BulkCopyRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerBulkCopyCommand(ConnectionProvider).ExecuteAsync(request, cancellationToken);
        }

        public override SqlNonQueryResult ExecuteNonQuery(SqlNonQueryRequest request)
        {
            return new SqlServerSqlNonQueryCommand(ConnectionProvider).Execute(request);
        }

        public override async Task<SqlNonQueryResult> ExecuteNonQueryAsync(SqlNonQueryRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerSqlNonQueryCommand(ConnectionProvider).ExecuteAsync(request, cancellationToken);
        }

        public override int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(new SqlNonQueryRequest(sql)).RowCountAffected;
        }

        public override async Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken)
        {
            return (await ExecuteNonQueryAsync(new SqlNonQueryRequest(sql), cancellationToken)).RowCountAffected;
        }

        public override TResult ExecuteScalar<TResult>(ScalarRequest request)
        {
            return new SqlServerScalarCommand<TResult>(ConnectionProvider).Execute(request);
        }

        public override async Task<TResult> ExecuteScalarAsync<TResult>(ScalarRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerScalarCommand<TResult>(ConnectionProvider).ExecuteAsync(request, cancellationToken);
        }

        public override TResult ExecuteScalar<TResult>(string sql)
        {
            return ExecuteScalar<TResult>(new ScalarRequest(sql));
        }

        public override async Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken)
        {
            return await ExecuteScalarAsync<TResult>(new ScalarRequest(sql), cancellationToken);
        }

        public override SqlQueryResult ExecuteSql(SqlQueryRequest request)
        {
            return new SqlServerSqlQueryCommand(ConnectionProvider, _commandExecutor).Execute(request);
        }

        public override async Task<SqlQueryResult> ExecuteSqlAsync(SqlQueryRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerSqlQueryCommand(ConnectionProvider, _commandExecutor).ExecuteAsync(request, cancellationToken);
        }

        public override DataTable ExecuteSql(string sql)
        {
            return ExecuteSql(new SqlQueryRequest(sql)).DataTable;
        }

        public override async Task<DataTable> ExecuteSqlAsync(string sql, CancellationToken cancellationToken)
        {
            return (await ExecuteSqlAsync(new SqlQueryRequest(sql), cancellationToken)).DataTable;
        }

        public override StoredProcedureResult ExecuteStoredProcedure(StoredProcedureRequest request)
        {
            return new SqlServerStoredProcedureCommand(ConnectionProvider, _commandExecutor).Execute(request);
        }

        public override async Task<StoredProcedureResult> ExecuteStoredProcedureAsync(StoredProcedureRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerStoredProcedureCommand(ConnectionProvider, _commandExecutor).ExecuteAsync(request, cancellationToken);
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
            return new SqlServerPaginationCommand(_builder, DatabaseCommandBuilder, _commandExecutor).Execute(request);
        }

        public override async Task<PaginationResult> PaginateAsync(PaginationRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerPaginationCommand(_builder, DatabaseCommandBuilder, _commandExecutor).ExecuteAsync(request, cancellationToken);
        }

        private string SqlSelectServerName => "SELECT @@SERVERNAME";
        private ISqlServerConnectionProvider ConnectionProvider => new SqlServerConnectionProvider(_builder);
    }
}
