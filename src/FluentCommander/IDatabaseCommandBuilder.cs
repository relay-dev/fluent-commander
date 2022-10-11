using FluentCommander.BulkCopy;
using FluentCommander.Pagination;
using FluentCommander.Scalar;
using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
using FluentCommander.StoredProcedure;

namespace FluentCommander
{
    public interface IDatabaseCommandBuilder
    {
        BulkCopyCommandBuilder ForBulkCopy();
        BulkCopyCommandBuilder<TEntity> ForBulkCopy<TEntity>();
        PaginationCommandBuilder ForPagination();
        ScalarCommand<TResult> ForScalar<TResult>(string sql);
        SqlNonQueryCommand ForSqlNonQuery(string sql);
        SqlQueryCommand ForSqlQuery(string sql);
        SqlQueryCommand<TEntity> ForSqlQuery<TEntity>(string sql);
        StoredProcedureCommandBuilder ForStoredProcedure(string storedProcedureName);
        StoredProcedureCommandBuilder<TEntity> ForStoredProcedure<TEntity>(string storedProcedureName);
    }
}