namespace FluentCommander.Database
{
    public class BulkCopyCommandResult
    {
        public BulkCopyCommandResult(int rowCountCopied)
        {
            RowCountCopied = rowCountCopied;
        }

        /// <summary>
        /// The count of the rows that were copied
        /// </summary>
        public int RowCountCopied { get; }
    }
}
