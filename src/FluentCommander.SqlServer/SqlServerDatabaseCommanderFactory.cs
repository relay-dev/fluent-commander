using FluentCommander.Core;
using Microsoft.Data.SqlClient;

namespace FluentCommander.SqlServer
{
    public class SqlServerDatabaseCommanderFactory : IDatabaseCommanderFactory
    {
        private readonly IConnectionStringCollection _connectionStringCollection;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;
        private readonly IDatabaseCommandFactory _commandFactory;

        public SqlServerDatabaseCommanderFactory(
            IConnectionStringCollection connectionStringCollection,
            DatabaseCommandBuilder databaseCommandBuilder,
            IDatabaseCommandFactory commandFactory)
        {
            _connectionStringCollection = connectionStringCollection;
            _databaseCommandBuilder = databaseCommandBuilder;
            _commandFactory = commandFactory;
        }

        public IDatabaseCommander Create(string connectionName = null)
        {
            connectionName ??= "DefaultConnection";

            var builder = new SqlConnectionStringBuilder(_connectionStringCollection.Get(connectionName));

            return new SqlServerDatabaseCommander(_commandFactory, builder, _databaseCommandBuilder);
        }
    }
}
