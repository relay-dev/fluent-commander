using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class CommandBuilder<TRequest, TBuilder, TResult> : IDatabaseCommand<TResult> where TBuilder : class where TRequest : DatabaseCommandRequest
    {
        private readonly DatabaseCommandRequest _commandCommandRequest;

        protected CommandBuilder(DatabaseCommandRequest databaseCommandCommandRequest)
        {
            _commandCommandRequest = databaseCommandCommandRequest;
        }

        public TBuilder Join(Transaction transaction)
        {
            _commandCommandRequest.Transaction = transaction;

            return this as TBuilder;
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
