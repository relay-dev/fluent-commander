using FluentCommander.Core.Property;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FluentCommander.Core.Ordering
{
    public class ColumnOrderingBuilder
    {
        private readonly IHaveColumnOrdering _request;

        public ColumnOrderingBuilder(IHaveColumnOrdering request)
        {
            _request = request;
            _request.ColumnOrdering = new ColumnOrdering();
        }

        public ColumnOrderingBuilder OrderBy(string columnName)
        {
            _request.ColumnOrdering.ColumnOrders.Add(new ColumnOrder(columnName, OrderDirection.Ascending));

            return this;
        }

        public ColumnOrderingBuilder OrderByDescending(string columnName)
        {
            _request.ColumnOrdering.ColumnOrders.Add(new ColumnOrder(columnName, OrderDirection.Descending));

            return this;
        }
    }

    public class ColumnOrderingBuilder<TEntity>
    {
        private readonly IHaveColumnOrdering _request;

        public ColumnOrderingBuilder(IHaveColumnOrdering request)
        {
            _request = request;
        }

        public ColumnOrderingBuilder<TEntity> Build(Action<PropertyMapBuilder<TEntity>> propertyMappingBuilder)
        {
            _request.ColumnOrdering = ParseToColumnOrdering(propertyMappingBuilder);

            return this;
        }

        private ColumnOrdering ParseToColumnOrdering(Action<PropertyMapBuilder<TEntity>> propertyMappingBuilder)
        {
            var columnOrdering = new ColumnOrdering();

            var propertyMapBuilder = new PropertyMapBuilder<TEntity>();

            propertyMappingBuilder(propertyMapBuilder);

            foreach (PropertyMetadata<TEntity, object> property in propertyMapBuilder.PropertyMap.PropertyMetadataCollection.Where(p => !p.IsIgnore))
            {
                var member = property.Selector.Body as MemberExpression;
                var unary = property.Selector.Body as UnaryExpression;

                string name = ((MemberExpression)(member ?? unary?.Operand))?.Member.Name;
                
                columnOrdering.ColumnOrders.Add(
                    new ColumnOrder
                    {
                        ColumnName = name,
                        Direction = property.OrderDirection
                    });
            }

            return columnOrdering;
        }
    }
}
