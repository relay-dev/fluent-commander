using FluentCommander.Core.CommandBuilders;
using FluentCommander.Core.Property;
using System;

namespace FluentCommander.StoredProcedure
{
    public abstract class StoredProcedureCommandBuilder<TEntity> : ParameterizedCommandBuilder<StoredProcedureRequest, StoredProcedureCommand<TEntity>, StoredProcedureResult<TEntity>>
    {
        protected Action<PropertyMapBuilder<TEntity>> MappingBuilder;
        protected readonly StoredProcedureRequest CommandRequest;

        protected StoredProcedureCommandBuilder(StoredProcedureRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public StoredProcedureCommand<TEntity> Name(string storedProcedureName)
        {
            CommandRequest.StoredProcedureName = storedProcedureName;

            return this as StoredProcedureCommand<TEntity>;
        }

        public StoredProcedureCommand<TEntity> Project(Action<PropertyMapBuilder<TEntity>> mappingBuilder)
        {
            MappingBuilder = mappingBuilder;

            return this as StoredProcedureCommand<TEntity>;
        }
    }
}
