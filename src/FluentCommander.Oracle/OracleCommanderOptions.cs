namespace FluentCommander.Oracle
{
    public class OracleCommanderOptions : FluentCommanderOptions
    {
        public OracleCommanderOptions UseOracle(string connectionString)
        {
            ConnectionString = connectionString;

            return this;
        }
    }
}
