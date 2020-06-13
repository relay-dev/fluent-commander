using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace FluentCommander.Core.Builders
{
    public abstract class CommandBuilder<TBuilder, TResult> : IDatabaseCommand<TResult> where TBuilder : class
    {
        private readonly DatabaseCommandRequest _request;

        protected CommandBuilder(DatabaseCommandRequest request)
        {
            _request = request;
        }

        public TBuilder Join(Transaction transaction)
        {
            _request.Transaction = transaction;

            return this as TBuilder;
        }

        public TBuilder Timeout(TimeSpan timeout)
        {
            _request.Timeout = timeout;

            return this as TBuilder;
        }

        public abstract TResult Execute();
        public abstract Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
