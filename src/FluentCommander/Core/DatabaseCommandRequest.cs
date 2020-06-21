using System;
using System.Transactions;
using FluentCommander.Core.Options;

namespace FluentCommander.Core
{
    public class DatabaseCommandRequest
    {
        /// <summary>
        /// Global options that apply to all commands
        /// </summary>
        public CommandOptions Options { get; set; }

        /// <summary>
        /// Number of seconds for the operation to complete before it times out
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        public Transaction Transaction { get; set; }
    }
}
