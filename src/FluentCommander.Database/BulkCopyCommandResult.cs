namespace FluentCommander.Database
{
    public class BulkCopyCommandResult
    {
        public int RowCountCopied { get; }

        public BulkCopyCommandResult(int rowCountCopied)
        {
            RowCountCopied = rowCountCopied;
        }
    }
}
