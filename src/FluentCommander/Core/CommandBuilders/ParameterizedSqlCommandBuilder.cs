using FluentCommander.SqlQuery;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class ParameterizedSqlCommandBuilder<TRequest, TBuilder, TResult> : ParameterizedInputCommandBuilder<TRequest, TBuilder, TResult> where TBuilder : class
    {
        protected readonly SqlRequest SqlRequest;

        protected ParameterizedSqlCommandBuilder()
        {
            SqlRequest = new SqlRequest();
        }

        public TBuilder Sql(string sql)
        {
            SqlRequest.Sql = sql;

            return this as TBuilder;
        }
    }
}
