using FluentCommander.BulkCopy;
using FluentCommander.Core;
using FluentCommander.Pagination;
using FluentCommander.Scalar;
using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
using FluentCommander.StoredProcedure;

namespace FluentCommander
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
        public BulkCopyCommand ForBulkCopy()
        {
            return _commandFactory.Create<BulkCopyCommand>();
        }

        /// <summary>
        /// Builds a command to paginate across a database table
        /// </summary>
        /// <returns>A pagination command builder</returns>
        public PaginationCommand ForPagination()
        {
            return _commandFactory.Create<PaginationCommand>();
        }

        /// <summary>
        /// Builds a command to execute a scalar
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>A scalar command builder</returns>
        public ScalarCommand<TResult> ForScalar<TResult>(string sql)
        {
            return _commandFactory.Create<ScalarCommand<TResult>>().Sql(sql);
        }

        /// <summary>
        /// Builds a command to execute a non-query
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>A non-query command builder</returns>
        public SqlNonQueryCommand ForSqlNonQuery(string sql)
        {
            return _commandFactory.Create<SqlNonQueryCommand>().Sql(sql);
        }

        /// <summary>
        /// Builds a command to execute a query
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>A SQL command builder</returns>
        public SqlQueryCommand ForSqlQuery(string sql)
        {
            return _commandFactory.Create<SqlQueryCommand>().Sql(sql);
        }

        /// <summary>
        /// Builds a command to execute a stored procedure
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute</param>
        /// <returns>A stored procedure command builder</returns>
        public StoredProcedureCommand ForStoredProcedure(string storedProcedureName)
        {
            return _commandFactory.Create<StoredProcedureCommand>().Name(storedProcedureName);
        }

        /// <summary>
        /// Builds a command to execute based on the command type
        /// </summary>
        /// <returns>A command builder of the requested type</returns>
        public TCommand ForCommand<TCommand>() where TCommand : IDatabaseCommand
        {
            return _commandFactory.Create<TCommand>();
        }
    }
}
