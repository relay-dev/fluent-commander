using FluentCommander.Core;
using FluentCommander.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FluentCommander.EntityFramework.SqlServer
{
    public class EntityFrameworkSqlServerDatabaseRequestHandlerFactory : IDatabaseEntityRequestHandlerFactory
    {
        private readonly DbContext _dbContext;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;
        private readonly IConnectionStringCollection _connectionStringCollection;
        private readonly ISqlServerCommandExecutor _commandExecutor;

        public EntityFrameworkSqlServerDatabaseRequestHandlerFactory(
            DbContext dbContext,
            DatabaseCommandBuilder databaseCommandBuilder,
            IConnectionStringCollection connectionStringCollection,
            ISqlServerCommandExecutor commandExecutor)
        {
            _dbContext = dbContext;
            _databaseCommandBuilder = databaseCommandBuilder;
            _connectionStringCollection = connectionStringCollection;
            _commandExecutor = commandExecutor;
        }

        /// <summary>
        /// Creates a new IDatabaseEntityRequestHandler instance
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the IConnectionStringCollection</param>
        /// <returns>A new IDatabaseEntityCommander instance</returns>
        public IDatabaseEntityRequestHandler Create(string connectionStringName = null)
        {
            var builder = new SqlConnectionStringBuilder(_connectionStringCollection.Get(connectionStringName));

            return new EntityFrameworkSqlServerDatabaseRequestHandler(_dbContext, _databaseCommandBuilder, builder, _commandExecutor);
        }
    }
}
