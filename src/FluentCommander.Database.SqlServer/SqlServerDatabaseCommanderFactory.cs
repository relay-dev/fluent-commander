using FluentCommander.Core;
using System.Data.SqlClient;

namespace FluentCommander.Database.SqlServer
{
    public class SqlServerDatabaseCommanderFactory : IDatabaseCommanderFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;

        public SqlServerDatabaseCommanderFactory(IConnectionStringProvider connectionStringProvider, DatabaseCommandBuilder databaseCommandBuilder)
        {
            _connectionStringProvider = connectionStringProvider;
            _databaseCommandBuilder = databaseCommandBuilder;
        }

        public IDatabaseCommander Create(string connectionName = null)
        {
            connectionName ??= "DefaultConnection";

            var builder = new SqlConnectionStringBuilder(_connectionStringProvider.Get(connectionName));

            return new SqlServerDatabaseCommander(builder, _databaseCommandBuilder);
        }
    }
}