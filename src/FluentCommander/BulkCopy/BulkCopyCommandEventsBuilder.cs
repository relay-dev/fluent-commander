using System;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyCommandEventsBuilder
    {
        private readonly BulkCopyRequest _request;

        public BulkCopyCommandEventsBuilder(BulkCopyRequest request)
        {
            _request = request;
        }

        public BulkCopyCommandEventsBuilder NotifyAfter(int rowCount)
        {
            _request.NotifyAfter = rowCount;

            return this;
        }

        public BulkCopyCommandEventsBuilder OnRowsCopied(Action<object, object> onRowsCopied)
        {
            _request.OnRowsCopied = onRowsCopied;

            return this;
        }
    }
}
