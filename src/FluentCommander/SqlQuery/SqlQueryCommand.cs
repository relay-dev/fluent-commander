using FluentCommander.Core.CommandBuilders;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlQuery
{
    public class SqlQueryCommand : ParameterizedSqlCommandBuilder<SqlQueryCommand, SqlQueryResult>
    {
        private readonly IDatabaseCommander _databaseCommander;

        public SqlQueryCommand(IDatabaseCommander databaseCommander)
            : base(new SqlRequest())
        {
            _databaseCommander = databaseCommander;
        }

        /// <summary>Executes the command</summary>
        /// <returns>The result of the command</returns>
        public override SqlQueryResult Execute()
        {
            return _databaseCommander.ExecuteSql(CommandRequest);
        }

        /// <summary>Executes the command asynchronously</summary>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public override async Task<SqlQueryResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _databaseCommander.ExecuteSqlAsync(CommandRequest, cancellationToken);
        }
    }
}