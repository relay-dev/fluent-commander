using FluentCommander.Core.Options;
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

        public TBuilder Join(SqlTransaction transactionScope)
        {
            _request.TransactionScope = transactionScope;

            return this as TBuilder;
        }

        public virtual TBuilder Options(Func<CommandOptionsBuilder, CommandOptionsBuilder> options)
        {
            options.Invoke(new CommandOptionsBuilder(_request));

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
