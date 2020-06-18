using FluentCommander.Core;
using FluentCommander.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FluentCommander.EntityFramework.SqlServer
{
    public class EntityFrameworkSqlServerDatabaseCommanderFactory : IDatabaseEntityCommanderFactory
    {
        private readonly DbContext _dbContext;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;
        private readonly IConnectionStringCollection _connectionStringCollection;
        private readonly ISqlServerCommandExecutor _commandExecutor;
        private readonly ILoggerFactory _loggerFactory;

        public EntityFrameworkSqlServerDatabaseCommanderFactory(
            DbContext dbContext,
            DatabaseCommandBuilder databaseCommandBuilder,
            IConnectionStringCollection connectionStringCollection,
            ISqlServerCommandExecutor commandExecutor,
            ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _databaseCommandBuilder = databaseCommandBuilder;
            _connectionStringCollection = connectionStringCollection;
            _commandExecutor = commandExecutor;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Creates a new IDatabaseCommander instance
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the IConnectionStringCollection</param>
        /// <returns>A new IDatabaseEntityCommander instance</returns>
        public IDatabaseEntityCommander Create(string connectionStringName = null)
        {
            var builder = new SqlConnectionStringBuilder(_connectionStringCollection.Get(connectionStringName));

            return new EntityFrameworkSqlServerDatabaseCommander(_dbContext, _databaseCommandBuilder, builder, _commandExecutor, _loggerFactory);
        }
    }
}
