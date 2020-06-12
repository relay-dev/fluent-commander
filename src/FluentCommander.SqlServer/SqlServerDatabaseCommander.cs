using FluentCommander.BulkCopy;
using FluentCommander.Pagination;
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

        public SqlServerDatabaseCommander(
            SqlConnectionStringBuilder builder,
            DatabaseCommandBuilder databaseCommandBuilder)
            : base(databaseCommandBuilder)
        {
            _builder = builder;
        }

        public override BulkCopyResult BulkCopy(BulkCopyRequest request)
        {
            return new SqlServerBulkCopyCommand(ConnectionProvider).Execute(request);
        }

        public override async Task<BulkCopyResult> BulkCopyAsync(BulkCopyRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerBulkCopyCommand(ConnectionProvider).ExecuteAsync(request, cancellationToken);
        }

        public override SqlNonQueryResult ExecuteNonQuery(SqlRequest request)
        {
            return new SqlServerSqlNonQueryCommand(ConnectionProvider).Execute(request);
        }

        public override async Task<SqlNonQueryResult> ExecuteNonQueryAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerSqlNonQueryCommand(ConnectionProvider).ExecuteAsync(request, cancellationToken);
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
            return new SqlServerScalarCommand<TResult>(ConnectionProvider).Execute(request);
        }

        public override async Task<TResult> ExecuteScalarAsync<TResult>(SqlRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerScalarCommand<TResult>(ConnectionProvider).ExecuteAsync(request, cancellationToken);
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
            return new SqlServerSqlQueryCommand(ConnectionProvider).Execute(request);
        }

        public override async Task<SqlQueryResult> ExecuteSqlAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerSqlQueryCommand(ConnectionProvider).ExecuteAsync(request, cancellationToken);
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
            return new SqlServerStoredProcedureCommand(ConnectionProvider).Execute(request);
        }

        public override async Task<StoredProcedureResult> ExecuteStoredProcedureAsync(StoredProcedureRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerStoredProcedureCommand(ConnectionProvider).ExecuteAsync(request, cancellationToken);
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
            return new SqlServerPaginationCommand(_builder, DatabaseCommandBuilder).Execute(request);
        }

        public override async Task<PaginationResult> PaginateAsync(PaginationRequest request, CancellationToken cancellationToken)
        {
            return await new SqlServerPaginationCommand(_builder, DatabaseCommandBuilder).ExecuteAsync(request, cancellationToken);
        }

        private string SqlSelectServerName => "SELECT @@SERVERNAME";
        private ISqlServerConnectionProvider ConnectionProvider => new SqlServerConnectionProvider(_builder);
    }
}
