using FluentCommander.Core.Options;
using System;
using System.Data;

namespace FluentCommander.Core
{
    public class DatabaseCommandRequest
    {
        /// <summary>
        /// Global options that apply to all commands
        /// </summary>
        public CommandOptions Options { get; set; }

        /// <summary>
        /// The amount of time to wait for the operation to complete
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// The transaction to join when executing the command
        /// </summary>
        public IDbTransaction Transaction { get; set; }
    }
}
