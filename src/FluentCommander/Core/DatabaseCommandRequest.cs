using System;
using System.Transactions;

namespace FluentCommander.Core
{
    public class DatabaseCommandRequest
    {
        /// <summary>
        /// Number of seconds for the operation to complete before it times out
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        public Transaction Transaction { get; set; }
    }
}
