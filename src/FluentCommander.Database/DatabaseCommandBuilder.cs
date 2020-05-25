using FluentCommander.Database.Commands;
using FluentCommander.Database.Core;

namespace FluentCommander.Database
{
    public class DatabaseCommandBuilder
    {
        private readonly IDatabaseCommandFactory _commandFactory;

        public DatabaseCommandBuilder(IDatabaseCommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        /// <summary>
        /// Builds a command to bulk copy into the database
        /// </summary>
        /// <returns>A bulk copy command builder</returns>
        public BulkCopyDatabaseCommand ForBulkCopy()
        {
            return _commandFactory.Create<BulkCopyDatabaseCommand>();
        }

        /// <summary>
        /// Builds a command to paginate across a database table
        /// </summary>
        /// <returns>A pagination command builder</returns>
        public PaginationDatabaseCommand ForPagination()
        {
            return _commandFactory.Create<PaginationDatabaseCommand>();
        }

        /// <summary>
        /// Builds a command to execute a scalar
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>A scalar command builder</returns>
        public ScalarDatabaseCommand<TResult> ForScalar<TResult>(string sql)
        {
            return _commandFactory.Create<ScalarDatabaseCommand<TResult>>().Sql(sql);
        }

        /// <summary>
        /// Builds a command to execute a non-query
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>A non-query command builder</returns>
        public SqlNonQueryDatabaseCommand ForSqlNonQuery(string sql)
        {
            return _commandFactory.Create<SqlNonQueryDatabaseCommand>().Sql(sql);
        }

        /// <summary>
        /// Builds a command to execute a query
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>A SQL command builder</returns>
        public SqlQueryDatabaseCommand ForSqlQuery(string sql)
        {
            return _commandFactory.Create<SqlQueryDatabaseCommand>().Sql(sql);
        }

        /// <summary>
        /// Builds a command to execute a stored procedure
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute</param>
        /// <returns>A stored procedure command builder</returns>
        public StoredProcedureDatabaseCommand ForStoredProcedure(string storedProcedureName)
        {
            return _commandFactory.Create<StoredProcedureDatabaseCommand>().Name(storedProcedureName);
        }
    }
}
