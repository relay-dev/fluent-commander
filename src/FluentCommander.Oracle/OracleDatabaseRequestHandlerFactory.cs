using FluentCommander.Core;
using Oracle.ManagedDataAccess.Client;

namespace FluentCommander.Oracle
{
    public class OracleDatabaseRequestHandlerFactory : IDatabaseRequestHandlerFactory
    {
        private readonly IConnectionStringCollection _connectionStringCollection;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;

        public OracleDatabaseRequestHandlerFactory(IConnectionStringCollection connectionStringCollection, DatabaseCommandBuilder databaseCommandBuilder)
        {
            _connectionStringCollection = connectionStringCollection;
            _databaseCommandBuilder = databaseCommandBuilder;
        }

        public IDatabaseRequestHandler Create(string connectionName = null)
        {
            connectionName ??= "DefaultConnection";

            var builder = new OracleConnectionStringBuilder(_connectionStringCollection.Get(connectionName));

            return new OracleDatabaseRequestHandler(builder, _databaseCommandBuilder);
        }
    }
}
