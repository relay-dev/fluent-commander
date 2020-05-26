namespace FluentCommander
{
    public class BulkCopyResult
    {
        public BulkCopyResult(int rowCountCopied)
        {
            RowCountCopied = rowCountCopied;
        }

        /// <summary>
        /// The count of the rows that were copied
        /// </summary>
        public int RowCountCopied { get; }
    }
}
