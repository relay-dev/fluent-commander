using FluentCommander.Core.Property;
using FluentCommander.Core.Utility.Impl;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.StoredProcedure
{
    public class StoredProcedureCommand<TEntity> : StoredProcedureCommandBuilder<TEntity>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private Action<PropertyMapBuilder<TEntity>> _mappingBuilder;

        public StoredProcedureCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public StoredProcedureCommand<TEntity> Project(Action<PropertyMapBuilder<TEntity>> mappingBuilder)
        {
            _mappingBuilder = mappingBuilder;

            return this;
        }

        // TODO: 
        public override StoredProcedureResult<TEntity> Execute()
        {
            CommandRequest.Parameters = Parameters;
            CommandRequest.Timeout = CommandTimeout;

            StoredProcedureResult storedProcedureResult =
                _databaseCommander.ExecuteStoredProcedure(CommandRequest);

            List<TEntity> result = MapToEntities(storedProcedureResult);

            return new StoredProcedureResult<TEntity>(result, storedProcedureResult.Parameters);
        }

        public override async Task<StoredProcedureResult<TEntity>> ExecuteAsync(CancellationToken cancellationToken)
        {
            CommandRequest.Parameters = Parameters;
            CommandRequest.Timeout = CommandTimeout;

            StoredProcedureResult storedProcedureResult = await 
                _databaseCommander.ExecuteStoredProcedureAsync(CommandRequest, cancellationToken);

            List<TEntity> result = MapToEntities(storedProcedureResult);

            return new StoredProcedureResult<TEntity>(result, storedProcedureResult.Parameters);
        }

        private List<TEntity> MapToEntities(StoredProcedureResult storedProcedureResult)
        {
            var options = new PropertyMapBuilder<TEntity>();

            _mappingBuilder(options);

            List<TEntity> result = ReflectionUtility.DataTableToList(storedProcedureResult.DataTable, options);

            return result;
        }
    }
}
