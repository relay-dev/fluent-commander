using FluentCommander.SqlQuery;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class ParameterizedSqlCommandBuilder<TRequest, TBuilder, TResult> : ParameterizedInputCommandBuilder<TRequest, TBuilder, TResult> where TBuilder : class where TRequest : DatabaseCommandRequest
    {
        protected readonly SqlRequest CommandRequest;

        protected ParameterizedSqlCommandBuilder(SqlRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public TBuilder Sql(string sql)
        {
            CommandRequest.Sql = sql;

            return this as TBuilder;
        }
    }
}
