using FluentCommander.Core.CommandBuilders;
using FluentCommander.Core.Property;
using System;

namespace FluentCommander.SqlQuery
{
    public abstract class SqlQueryCommandBuilder<TEntity> : ParameterizedSqlCommandBuilder<SqlQueryCommand<TEntity>, SqlQueryResult<TEntity>>
    {
        protected Action<PropertyMapBuilder<TEntity>> MappingBuilder;

        protected SqlQueryCommandBuilder(SqlRequest commandRequest) 
            : base(commandRequest) { }

        public SqlQueryCommandBuilder<TEntity> Project(Action<PropertyMapBuilder<TEntity>> mappingBuilder)
        {
            MappingBuilder = mappingBuilder;

            return this;
        }
    }
}
