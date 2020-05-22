using FluentCommander.Database.Commands;

namespace FluentCommander.Database
{
    public class DatabaseCommandBuilder
    {
        private readonly IDatabaseCommandFactory _commandFactory;

        public DatabaseCommandBuilder(IDatabaseCommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public BulkCopyDatabaseCommand ForBulkCopy()
        {
            return _commandFactory.Create<BulkCopyDatabaseCommand>();
        }

        public PaginationDatabaseCommand ForPagination()
        {
            return _commandFactory.Create<PaginationDatabaseCommand>();
        }

        public SqlNonQueryDatabaseCommand ForSqlNonQuery(string sql)
        {
            return _commandFactory.Create<SqlNonQueryDatabaseCommand>().Sql(sql);
        }

        public SqlQueryDatabaseCommand ForSqlQuery(string sql)
        {
            return _commandFactory.Create<SqlQueryDatabaseCommand>().Sql(sql);
        }

        public StoredProcedureDatabaseCommand ForStoredProcedure(string storedProcedureName)
        {
            return _commandFactory.Create<StoredProcedureDatabaseCommand>().Named(storedProcedureName);
        }
    }
}
