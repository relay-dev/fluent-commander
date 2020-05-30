﻿namespace FluentCommander.EntityFramework
{
    public class PropertyMetadataBuilder<TEntity>
    {
        public readonly PropertyMetadata<TEntity, object> PropertyMetadata;

        public PropertyMetadataBuilder(PropertyMetadata<TEntity, object> propertyMetadata)
        {
            PropertyMetadata = propertyMetadata;
        }

        public PropertyMetadataBuilder<TEntity> Name(string value)
        {
            PropertyMetadata.Name = value;

            return this;
        }

        public PropertyMetadataBuilder<TEntity> MapTo(string value)
        {
            PropertyMetadata.MapTo = value;

            return this;
        }

        public PropertyMetadataBuilder<TEntity> Key()
        {
            PropertyMetadata.IsKey = true;

            return this;
        }

        public PropertyMetadataBuilder<TEntity> Ignore()
        {
            PropertyMetadata.IsIgnored = true;

            return this;
        }
    }
}
