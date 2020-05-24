using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database
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
        /// <param name="tableName">The name of the table to insert into</param>
        /// <param name="dataTable">The dataTable that contains the data to be inserted</param>
        /// <param name="columnMapping">Optional; maps the dataTable column names to the database table column names</param>
        BulkCopyResult BulkCopy(string tableName, DataTable dataTable, ColumnMapping columnMapping);

        /// <summary>
        /// Inserts a set of records from a <see cref="DataTable"/> into a database table in a single transaction
        /// </summary>
        /// <param name="tableName">The name of the table to insert into</param>
        /// <param name="dataTable">The dataTable that contains the data to be inserted</param>
        /// <param name="columnMapping">Optional; maps the dataTable column names to the database table column names</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        Task<BulkCopyResult> BulkCopyAsync(string tableName, DataTable dataTable, ColumnMapping columnMapping, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a SQL statement which is either an Insert, Update or Delete
        /// </summary>
        /// <param name="sql">The SQL to be executed</param>
        /// <param name="databaseParameters">Optional; if there are parameter placeholders in the SQL string, the parameter values should be specified here</param>
        /// <returns>The count of rows affected</returns>
        int ExecuteNonQuery(string sql, List<DatabaseCommandParameter> databaseParameters = null);

        /// <summary>
        /// Executes a SQL statement which is either an Insert, Update or Delete
        /// </summary>
        /// <param name="sql">The SQL to be executed</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <param name="databaseParameters">Optional; if there are parameter placeholders in the SQL string, the parameter values should be specified here</param>
        /// <returns>The count of rows affected</returns>
        Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken, List<DatabaseCommandParameter> databaseParameters = null);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to which returns a single value
        /// </summary>
        /// <typeparam name="TResult">The type to convert the result to</typeparam>
        /// <param name="sql">The SQL to be executed</param>
        /// <param name="databaseParameters">Optional; if there are parameter placeholders in the SQL string, the parameter values should be specified here</param>
        /// <returns>The result of the SQL string</returns>
        TResult ExecuteScalar<TResult>(string sql, List<DatabaseCommandParameter> databaseParameters = null);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to which returns a single value
        /// </summary>
        /// <typeparam name="TResult">The type to convert the result to</typeparam>
        /// <param name="sql">The SQL to be executed</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <param name="databaseParameters">Optional; if there are parameter placeholders in the SQL string, the parameter values should be specified here</param>
        /// <returns>The result of the SQL string</returns>
        Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken, List<DatabaseCommandParameter> databaseParameters = null);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="sql">The SQL to be executed</param>
        /// <param name="databaseParameters">Optional; if there are parameter placeholders in the SQL string, the parameter values should be specified here</param>
        /// <returns>The result of the SQL string</returns>
        DataTable ExecuteSql(string sql, List<DatabaseCommandParameter> databaseParameters = null);

        /// <summary>
        /// Executes a SQL string against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="sql">The SQL to be executed</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <param name="databaseParameters">Optional; if there are parameter placeholders in the SQL string, the parameter values should be specified here</param>
        /// <returns>The result of the SQL string</returns>
        Task<DataTable> ExecuteSqlAsync(string sql, CancellationToken cancellationToken, List<DatabaseCommandParameter> databaseParameters = null);

        /// <summary>
        /// Executes a stored procedure against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to be executed</param>
        /// <param name="databaseParameters">Optional; if there are parameter placeholders in the SQL string, the parameter values should be specified here</param>
        /// <returns>The result of the stored procedure</returns>
        StoredProcedureResult ExecuteStoredProcedure(string storedProcedureName, List<DatabaseCommandParameter> databaseParameters = null);

        /// <summary>
        /// Executes a stored procedure against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to be executed</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <param name="databaseParameters">Optional; if there are parameter placeholders in the SQL string, the parameter values should be specified here</param>
        /// <returns>The result of the stored procedure</returns>
        Task<StoredProcedureResult> ExecuteStoredProcedureAsync(string storedProcedureName, CancellationToken cancellationToken, List<DatabaseCommandParameter> databaseParameters = null);

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
        /// <param name="paginationRequest">The settings needed to execute a pagination command request</param>
        /// <returns>The result of the query</returns>
        PaginationResult Paginate(PaginationRequest paginationRequest);

        /// <summary>
        /// Executes a pagination query against the database this <see cref="IDatabaseCommander"/> is connected to
        /// </summary>
        /// <param name="paginationRequest">The settings needed to execute a pagination command request</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the query</returns>
        Task<PaginationResult> PaginateAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);

        /// <summary>
        /// The database command timeout in seconds
        /// </summary>
        int TimeoutInSeconds { get; set; }
    }
}
