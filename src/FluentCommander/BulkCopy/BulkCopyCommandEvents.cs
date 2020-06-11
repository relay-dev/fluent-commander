using System;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyCommandEvents
    {
        /// <summary>
        /// Defines the number of rows to be processed before generating a notification event
        /// </summary>
        public int? NotifyAfter { get; set; }

        /// <summary>
        /// Delegate to be invoked every time that the number of rows specified by the NotifyAfter property have been processed.
        /// </summary>
        public Action<object, object> OnRowsCopied { get; set; }
    }
}
