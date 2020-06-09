using System;
using System.Transactions;

namespace FluentCommander.Core
{
    public class DatabaseCommandRequest
    {
        /// <summary>
        /// Sets the timeout for one specific command request
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        public Transaction Transaction { get; set; }
    }
}
