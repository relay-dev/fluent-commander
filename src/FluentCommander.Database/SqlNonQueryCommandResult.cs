namespace FluentCommander.Database
{
    public class SqlNonQueryCommandResult
    {
        public SqlNonQueryCommandResult(int rowCountAffected)
        {
            RowCountAffected = rowCountAffected;
        }

        /// <summary>
        /// The count of the rows that were affected by the non-query
        /// </summary>
        public int RowCountAffected { get; }
    }
}
