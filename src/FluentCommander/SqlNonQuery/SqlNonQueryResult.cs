namespace FluentCommander.SqlNonQuery
{
    public class SqlNonQueryResult
    {
        public SqlNonQueryResult(int rowCountAffected)
        {
            RowCountAffected = rowCountAffected;
        }

        /// <summary>
        /// The count of the rows that were affected by the non-query
        /// </summary>
        public int RowCountAffected { get; }
    }
}
