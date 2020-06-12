using FluentCommander.BulkCopy;
using FluentCommander.Pagination;
using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
using FluentCommander.StoredProcedure;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander
{
    public abstract class DatabaseCommanderBase : IDatabaseCommander
    {
        protected readonly DatabaseCommandBuilder DatabaseCommandBuilder;

        protected DatabaseCommanderBase(DatabaseCommandBuilder databaseCommandBuilder)
        {
            DatabaseCommandBuilder = databaseCommandBuilder;
        }

        public DatabaseCommandBuilder BuildCommand()
        {
            return DatabaseCommandBuilder;
        }

        public abstract BulkCopyResult BulkCopy(BulkCopyRequest request);
        public abstract Task<BulkCopyResult> BulkCopyAsync(BulkCopyRequest request, CancellationToken cancellationToken);
        public abstract SqlNonQueryResult ExecuteNonQuery(SqlRequest request);
        public abstract Task<SqlNonQueryResult> ExecuteNonQueryAsync(SqlRequest request, CancellationToken cancellationToken);
        public abstract int ExecuteNonQuery(string sql);
        public abstract Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken);
        public abstract TResult ExecuteScalar<TResult>(SqlRequest request);
        public abstract Task<TResult> ExecuteScalarAsync<TResult>(SqlRequest request, CancellationToken cancellationToken);
        public abstract TResult ExecuteScalar<TResult>(string sql);
        public abstract Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken);
        public abstract SqlQueryResult ExecuteSql(SqlRequest request);
        public abstract Task<SqlQueryResult> ExecuteSqlAsync(SqlRequest request, CancellationToken cancellationToken);
        public abstract DataTable ExecuteSql(string sql);
        public abstract Task<DataTable> ExecuteSqlAsync(string sql, CancellationToken cancellationToken);
        public abstract StoredProcedureResult ExecuteStoredProcedure(StoredProcedureRequest request);
        public abstract Task<StoredProcedureResult> ExecuteStoredProcedureAsync(StoredProcedureRequest request, CancellationToken cancellationToken);
        public abstract string GetServerName();
        public abstract Task<string> GetServerNameAsync(CancellationToken cancellationToken);
        public abstract PaginationResult Paginate(PaginationRequest request);
        public abstract Task<PaginationResult> PaginateAsync(PaginationRequest request, CancellationToken cancellationToken);
    }
}
