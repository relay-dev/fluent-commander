using FluentCommander.Core;
using Oracle.ManagedDataAccess.Client;

namespace FluentCommander.Oracle
{
    public class OracleDatabaseCommanderFactory : IDatabaseCommanderFactory
    {
        private readonly IConnectionStringCollection _connectionStringCollection;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;

        public OracleDatabaseCommanderFactory(IConnectionStringCollection connectionStringCollection, DatabaseCommandBuilder databaseCommandBuilder)
        {
            _connectionStringCollection = connectionStringCollection;
            _databaseCommandBuilder = databaseCommandBuilder;
        }

        public IDatabaseCommander Create(string connectionName = null)
        {
            connectionName ??= "DefaultConnection";

            var builder = new OracleConnectionStringBuilder(_connectionStringCollection.Get(connectionName));

            return new OracleDatabaseCommander(builder, _databaseCommandBuilder);
        }
    }
}
