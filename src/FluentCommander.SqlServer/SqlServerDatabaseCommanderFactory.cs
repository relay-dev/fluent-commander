using FluentCommander.Core;
using Microsoft.Data.SqlClient;

namespace FluentCommander.SqlServer
{
    public class SqlServerDatabaseCommanderFactory : IDatabaseCommanderFactory
    {
        private readonly IConnectionStringCollection _connectionStringCollection;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;

        public SqlServerDatabaseCommanderFactory(IConnectionStringCollection connectionStringCollection, DatabaseCommandBuilder databaseCommandBuilder)
        {
            _connectionStringCollection = connectionStringCollection;
            _databaseCommandBuilder = databaseCommandBuilder;
        }

        public IDatabaseCommander Create(string connectionName = null)
        {
            connectionName ??= "DefaultConnection";

            var builder = new SqlConnectionStringBuilder(_connectionStringCollection.Get(connectionName));

            return new SqlServerDatabaseCommander(builder, _databaseCommandBuilder);
        }
    }
}
