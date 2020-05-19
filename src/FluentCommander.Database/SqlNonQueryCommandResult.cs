namespace FluentCommander.Database
{
    public class SqlNonQueryCommandResult
    {
        public int RowCountAffected { get; }

        public SqlNonQueryCommandResult(int rowCountAffected)
        {
            RowCountAffected = rowCountAffected;
        }
    }
}
