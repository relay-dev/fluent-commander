using FluentCommander.Core.Property;
using FluentCommander.Core.Utility.Impl;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.StoredProcedure
{
    public class StoredProcedureCommand<TEntity> : StoredProcedureCommandBuilder<TEntity>
    {
        private readonly IDatabaseRequestHandler _databaseRequestHandler;

        public StoredProcedureCommand(IDatabaseRequestHandler databaseRequestHandler)
            : base(new StoredProcedureRequest())
        {
            _databaseRequestHandler = databaseRequestHandler;
        }

        public override StoredProcedureResult<TEntity> Execute()
        {
            StoredProcedureResult storedProcedureResult =
                _databaseRequestHandler.ExecuteStoredProcedure(CommandRequest);

            List<TEntity> data = MapToEntities(storedProcedureResult.DataTable);

            return new StoredProcedureResult<TEntity>(data, storedProcedureResult.Parameters);
        }

        public override async Task<StoredProcedureResult<TEntity>> ExecuteAsync(CancellationToken cancellationToken)
        {
            StoredProcedureResult storedProcedureResult = await 
                _databaseRequestHandler.ExecuteStoredProcedureAsync(CommandRequest, cancellationToken);

            List<TEntity> data = MapToEntities(storedProcedureResult.DataTable);

            return new StoredProcedureResult<TEntity>(data, storedProcedureResult.Parameters);
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
