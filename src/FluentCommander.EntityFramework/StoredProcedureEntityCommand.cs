using FluentCommander.EntityFramework.Internal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Core.CommandBuilders;
using FluentCommander.StoredProcedure;

namespace FluentCommander.EntityFramework
{
    public class StoredProcedureEntityCommand<TEntity> : ParameterizedCommandBuilder<StoredProcedureEntityCommand<TEntity>, StoredProcedureEntityResult<TEntity>>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly StoredProcedureRequest _storedProcedureRequest;
        private Action<PropertyMapBuilder<TEntity>> _mappingBuilder;

        public StoredProcedureEntityCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
            _storedProcedureRequest = new StoredProcedureRequest();
        }

        public StoredProcedureEntityCommand<TEntity> Name(string storedProcedureName)
        {
            _storedProcedureRequest.StoredProcedureName = storedProcedureName;

            return this;
        }

        public StoredProcedureEntityCommand<TEntity> Project(Action<PropertyMapBuilder<TEntity>> mappingBuilder)
        {
            _mappingBuilder = mappingBuilder;

            return this;
        }

        // TODO: 
        public override StoredProcedureEntityResult<TEntity> Execute()
        {
            _storedProcedureRequest.DatabaseParameters = Parameters;
            _storedProcedureRequest.Timeout = CommandTimeout;

            StoredProcedureResult storedProcedureResult =
                _databaseCommander.ExecuteStoredProcedure(_storedProcedureRequest);

            List<TEntity> result = MapToEntities(storedProcedureResult);

            return new StoredProcedureEntityResult<TEntity>(storedProcedureResult.Parameters, storedProcedureResult.DataTable, result);
        }

        public override async Task<StoredProcedureEntityResult<TEntity>> ExecuteAsync(CancellationToken cancellationToken)
        {
            _storedProcedureRequest.DatabaseParameters = Parameters;
            _storedProcedureRequest.Timeout = CommandTimeout;

            StoredProcedureResult storedProcedureResult = await 
                _databaseCommander.ExecuteStoredProcedureAsync(_storedProcedureRequest, cancellationToken);

            List<TEntity> result = MapToEntities(storedProcedureResult);

            return new StoredProcedureEntityResult<TEntity>(storedProcedureResult.Parameters, storedProcedureResult.DataTable, result);
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
