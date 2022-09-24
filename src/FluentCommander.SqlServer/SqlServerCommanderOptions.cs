namespace FluentCommander.SqlServer
{
    public class SqlServerCommanderOptions : FluentCommanderOptions
    {
        public SqlServerCommanderOptions UseSqlServer(string connectionString)
        {
            ConnectionString = connectionString;

            return this;
        }
    }
}
