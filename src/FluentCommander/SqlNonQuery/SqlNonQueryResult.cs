namespace FluentCommander.SqlNonQuery
{
    public class SqlNonQueryResult
    {
        public SqlNonQueryResult(int rowCountAffected)
        {
            RowCountAffected = rowCountAffected;
        }

        public SqlNonQueryResult(int rowCountAffected, long id)
        {
            RowCountAffected = rowCountAffected;
            Id = id;
        }

        /// <summary>
        /// The primary key ID of the row that was inserted, if the command was an insert statement
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// The count of the rows that were affected by the non-query
        /// </summary>
        public int RowCountAffected { get; }
    }
}
