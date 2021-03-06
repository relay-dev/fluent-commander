﻿using System;
using System.Linq.Expressions;

namespace FluentCommander.Core.Property
{
    public class PropertyMapBuilder<TEntity>
    {
        internal PropertyMap<TEntity> PropertyMap { get; }

        public PropertyMapBuilder()
        {
            PropertyMap = new PropertyMap<TEntity>();
        }

        public PropertyMetadataBuilder<TEntity> Property(Expression<Func<TEntity, object>> propertySelector)
        {
            var propertyMetadata = new PropertyMetadata<TEntity, object>
            {
                Selector = propertySelector
            };

            PropertyMap.PropertyMetadataCollection.Add(propertyMetadata);

            return new PropertyMetadataBuilder<TEntity>(propertyMetadata);
        }
    }
}
