using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class CommandBuilder<TBuilder, TResult> : IDatabaseCommand<TResult> where TBuilder : class
    {
        protected TimeSpan CommandTimeout;

        public TBuilder Timeout(TimeSpan timeout)
        {
            CommandTimeout = timeout;

            return this as TBuilder;
        }

        public abstract TResult Execute();
        public abstract Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
