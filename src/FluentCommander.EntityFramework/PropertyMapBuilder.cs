using System;
using System.Linq.Expressions;

namespace FluentCommander.EntityFramework
{
    public class PropertyMapBuilder<TEntity>
    {
        public PropertyMap<TEntity> PropertyMap { get; }

        public PropertyMapBuilder()
        {
            PropertyMap = new PropertyMap<TEntity>();
        }

        public PropertyMetadataBuilder<TEntity> Property(Expression<Func<TEntity, object>> propertySelector)
        {
            var propertyMap = new PropertyMetadata<TEntity, object>
            {
                Selector = propertySelector
            };

            PropertyMap.PropertyMetadata.Add(propertyMap);

            var builder = new PropertyMetadataBuilder<TEntity>(propertyMap);

            return builder;
        }
    }
}
