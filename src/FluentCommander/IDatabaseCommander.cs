using FluentCommander.BulkCopy;
using FluentCommander.Core;
using FluentCommander.Pagination;
using FluentCommander.Scalar;
using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
using FluentCommander.StoredProcedure;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander
{
    /// <summary>
    /// A generic abstraction of a database
    /// </summary>
    public interface IDatabaseCommander
    {
        /// <summary>
        /// Initiates a database command
        /// </summary>
        /// <returns>A builder object that defines which command should be run</returns>
        DatabaseCommandBuilder BuildCommand();

        /// <summary>
        /// Inserts a set of records from a <see cref="DataTable"/> into a database table in a single transaction
        /// </summary>
        /// <param name="request">The data needed to execute the bulk copy command</param>
        /// <returns>The result of the command</returns>
        BulkCopyResult BulkCopy(BulkCopyRequest request);

        /// <summary>
        /// Inserts a set of records from a <see cref="DataTable"/> into a database table in a single transaction
        /// </summary>
        /// <param name="request">The data needed to execute the bulk copy command</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the command</returns>
        Task<BulkCopyResult> BulkCopyAsync(BulkCopyRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a SQL statement which is either an Insert, Update or Delete
        /// </summary>
        /// <param name="request">The data needed to execute the non-query command</param>
        /// <returns>The result of the command</returns>
        SqlNonQueryResult ExecuteNonQuery(SqlNonQueryRequest request);

        /// <summary>
        /// Executes a SQL statement which is either an Insert, Update or Delete
        /// </summary>
        /// <param name="request">The data needed to execute the non-query command</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the command</returns>
        Task<SqlNonQueryResult> ExecuteNonQueryAsync(SqlNonQueryRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a SQL statement which is either an Insert, Update or Delete
        /// </summary>
        /// <param name="sql">The SQL to be executed</param>
        /// <returns>The count of rows affected</returns>
        int ExecuteNonQuery(string sql);

        /// <summary>
        /// Executes a SQL statement which is either an Insert, Update or Delete
        /// </summary>
        /// <param name="sql">The SQL to be executed</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The count of rows affected</returns>
        Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to which returns a single value
        /// </summary>
        /// <typeparam name="TResult">The type to convert the result to</typeparam>
        /// <param name="request">The data needed to execute the scalar command</param>
        /// <returns>The result of the SQL string</returns>
        TResult ExecuteScalar<TResult>(ScalarRequest request);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to which returns a single value
        /// </summary>
        /// <typeparam name="TResult">The type to convert the result to</typeparam>
        /// <param name="request">The data needed to execute the scalar command</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the SQL string</returns>
        Task<TResult> ExecuteScalarAsync<TResult>(ScalarRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to which returns a single value
        /// </summary>
        /// <typeparam name="TResult">The type to convert the result to</typeparam>
        /// <param name="sql">The SQL to be executed</param>
        /// <returns>The result of the SQL string</returns>
        TResult ExecuteScalar<TResult>(string sql);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to which returns a single value
        /// </summary>
        /// <typeparam name="TResult">The type to convert the result to</typeparam>
        /// <param name="sql">The SQL to be executed</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the SQL string</returns>
        Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="request">The data needed to execute the non-query command</param>
        /// <returns>The result of the command</returns>
        SqlQueryResult ExecuteSql(SqlQueryRequest request);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="request">The data needed to execute the non-query command</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the command</returns>
        Task<SqlQueryResult> ExecuteSqlAsync(SqlQueryRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="sql">The SQL to be executed</param>
        /// <returns>The result of the SQL string</returns>
        DataTable ExecuteSql(string sql);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="sql">The SQL to be executed</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the SQL string</returns>
        Task<DataTable> ExecuteSqlAsync(string sql, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a stored procedure against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="request">The data needed to execute the stored procedure command</param>
        /// <returns>The result of the stored procedure</returns>
        StoredProcedureResult ExecuteStoredProcedure(StoredProcedureRequest request);

        /// <summary>
        /// Executes a stored procedure against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="request">The data needed to execute the stored procedure command</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the stored procedure</returns>
        Task<StoredProcedureResult> ExecuteStoredProcedureAsync(StoredProcedureRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the server name this <see cref="IDatabaseCommander"/> instance is connected to
        /// </summary>
        /// <returns>The server name</returns>
        string GetServerName();

        /// <summary>
        /// Gets the server name this <see cref="IDatabaseCommander"/> instance is connected to
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The server name</returns>
        Task<string> GetServerNameAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Executes a pagination query against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="request">The settings needed to execute a pagination command</param>
        /// <returns>The result of the query</returns>
        PaginationResult Paginate(PaginationRequest request);

        /// <summary>
        /// Executes a pagination query against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="request">The settings needed to execute a pagination command</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the query</returns>
        Task<PaginationResult> PaginateAsync(PaginationRequest request, CancellationToken cancellationToken);
    }
}
