using FluentCommander.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FluentCommander.EntityFramework.SqlServer
{
    public class EntityFrameworkSqlServerDatabaseCommanderFactory : IDatabaseEntityCommanderFactory
    {
        private readonly DbContext _dbContext;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;
        private readonly IConnectionStringCollection _connectionStringCollection;

        public EntityFrameworkSqlServerDatabaseCommanderFactory(
            DbContext dbContext,
            DatabaseCommandBuilder databaseCommandBuilder,
            IConnectionStringCollection connectionStringCollection)
        {
            _dbContext = dbContext;
            _databaseCommandBuilder = databaseCommandBuilder;
            _connectionStringCollection = connectionStringCollection;
        }

        /// <summary>
        /// Creates a new IDatabaseCommander instance
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the IConnectionStringCollection</param>
        /// <returns>A new IDatabaseEntityCommander instance</returns>
        public IDatabaseEntityCommander Create(string connectionStringName = null)
        {
            var builder = new SqlConnectionStringBuilder(_connectionStringCollection.Get(connectionStringName));

            return new EntityFrameworkSqlServerDatabaseCommander(_dbContext, _databaseCommandBuilder, builder);
        }
    }
}
