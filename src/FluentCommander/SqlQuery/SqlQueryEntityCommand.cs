using FluentCommander.Core.Property;
using FluentCommander.Core.Utility.Impl;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlQuery
{
    public class SqlQueryCommand<TEntity> : SqlQueryCommandBuilder<TEntity>
    {
        private readonly IDatabaseRequestHandler _databaseRequestHandler;

        public SqlQueryCommand(IDatabaseRequestHandler databaseRequestHandler)
            : base(new SqlQueryRequest())
        {
            _databaseRequestHandler = databaseRequestHandler;
        }

        /// <summary>Executes the command</summary>
        /// <returns>The result of the command</returns>
        public override SqlQueryResult<TEntity> Execute()
        {
            SqlQueryResult result = _databaseRequestHandler.ExecuteSql((SqlQueryRequest)CommandRequest);

            List<TEntity> entities = MapToEntities(result.DataTable);

            return new SqlQueryResult<TEntity>(entities);
        }

        /// <summary>Executes the command asynchronously</summary>
        /// <param name="cancellationToken">The cancellation token in scope for the operation</param>
        /// <returns>The result of the command</returns>
        public override async Task<SqlQueryResult<TEntity>> ExecuteAsync(CancellationToken cancellationToken)
        {
            SqlQueryResult result = await _databaseRequestHandler.ExecuteSqlAsync((SqlQueryRequest)CommandRequest, cancellationToken);

            List<TEntity> entities = MapToEntities(result.DataTable);

            return new SqlQueryResult<TEntity>(entities);
        }

        private List<TEntity> MapToEntities(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return new List<TEntity>();
            }

            var options = new PropertyMapBuilder<TEntity>();

            MappingBuilder(options);

            return ReflectionUtility.DataTableToList(dataTable, options);
        }
    }
}
