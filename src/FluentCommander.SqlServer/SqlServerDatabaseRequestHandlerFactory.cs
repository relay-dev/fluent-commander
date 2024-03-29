﻿using FluentCommander.Core;
using Microsoft.Data.SqlClient;

namespace FluentCommander.SqlServer
{
    public class SqlServerDatabaseRequestHandlerFactory : IDatabaseRequestHandlerFactory
    {
        private readonly IConnectionStringCollection _connectionStringCollection;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;
        private readonly ISqlServerCommandExecutor _sqlServerCommandExecutor;

        public SqlServerDatabaseRequestHandlerFactory(
            IConnectionStringCollection connectionStringCollection,
            DatabaseCommandBuilder databaseCommandBuilder,
            ISqlServerCommandExecutor sqlServerCommandExecutor)
        {
            _connectionStringCollection = connectionStringCollection;
            _databaseCommandBuilder = databaseCommandBuilder;
            _sqlServerCommandExecutor = sqlServerCommandExecutor;
        }

        /// <summary>
        /// Creates a new IDatabaseCommander instance
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the IConnectionStringCollection</param>
        /// <returns>A new IDatabaseEntityCommander instance</returns>
        public IDatabaseRequestHandler Create(string connectionStringName = "DefaultConnection")
        {
            var builder = new SqlConnectionStringBuilder(_connectionStringCollection.Get(connectionStringName));

            return new SqlServerDatabaseRequestHandler(builder, _databaseCommandBuilder, _sqlServerCommandExecutor);
        }
    }
}
