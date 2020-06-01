using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class CommandBuilder<TRequest, TBuilder, TResult> : IDatabaseCommand<TResult> where TBuilder : class where TRequest : DatabaseCommandRequest
    {
        private readonly DatabaseCommandRequest _commandCommandRequest;

        protected CommandBuilder(DatabaseCommandRequest databaseCommandCommandRequest)
        {
            _commandCommandRequest = databaseCommandCommandRequest;
        }

        public TBuilder Timeout(TimeSpan timeout)
        {
            _commandCommandRequest.Timeout = timeout;

            return this as TBuilder;
        }

        public abstract TResult Execute();
        public abstract Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
