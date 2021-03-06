﻿using FluentCommander.Core.Behaviors;
using FluentCommander.Core.Property;
using System;

namespace FluentCommander.StoredProcedure
{
    public abstract class StoredProcedureCommandBuilder<TEntity> : StoredProcedureCommandBuilderBase<StoredProcedureCommand<TEntity>, StoredProcedureResult<TEntity>>
    {
        protected Action<PropertyMapBuilder<TEntity>> MappingBuilder;
        protected readonly StoredProcedureRequest CommandRequest;

        protected StoredProcedureCommandBuilder(StoredProcedureRequest request)
            : base(request)
        {
            CommandRequest = request;
        }

        public StoredProcedureCommandBuilder<TEntity> Behaviors(Func<ReadBehaviorsBuilder, ReadBehaviorsBuilder> options)
        {
            options.Invoke(new ReadBehaviorsBuilder(CommandRequest));

            return this;
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
