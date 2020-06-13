using FluentCommander.Core.Behaviors;
using FluentCommander.Core.Builders;
using FluentCommander.Core.Property;
using System;

namespace FluentCommander.SqlQuery
{
    public abstract class SqlQueryCommandBuilder<TEntity> : ParameterizedSqlCommandBuilder<SqlQueryCommand<TEntity>, SqlQueryResult<TEntity>>
    {
        protected Action<PropertyMapBuilder<TEntity>> MappingBuilder;

        protected SqlQueryCommandBuilder(SqlQueryRequest commandRequest) 
            : base(commandRequest) { }

        public SqlQueryCommandBuilder<TEntity> Behaviors(Func<ReadBehaviorsBuilder, ReadBehaviorsBuilder> options)
        {
            options.Invoke(new ReadBehaviorsBuilder((SqlQueryRequest)CommandRequest));

            return this;
        }

        public SqlQueryCommandBuilder<TEntity> Project(Action<PropertyMapBuilder<TEntity>> mappingBuilder)
        {
            MappingBuilder = mappingBuilder;

            return this;
        }
    }
}
