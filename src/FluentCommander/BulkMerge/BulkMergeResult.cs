using FluentCommander.BulkCopy;

namespace FluentCommander.BulkMerge
{
    public class BulkMergeResult : BulkCopyResult
    {
        public BulkMergeResult(int rowCountCopied)
            : base(rowCountCopied) { }
    }
}
