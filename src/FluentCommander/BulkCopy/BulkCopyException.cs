using System;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyException : Exception
    {
        public long RowsCopied { get; }

        public BulkCopyException(string message)
            : base(message) { }

        public BulkCopyException(string message, Exception innerException)
            : base(message, innerException) { }

        public BulkCopyException(string message, long rowsCopied)
            : base(message)
        {
            RowsCopied = rowsCopied;
        }
    }
}
