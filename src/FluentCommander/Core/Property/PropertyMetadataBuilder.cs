using FluentCommander.Core.Ordering;

namespace FluentCommander.Core.Property
{
    public class PropertyMetadataBuilder<TEntity>
    {
        private readonly PropertyMetadata<TEntity, object> _propertyMetadata;

        public PropertyMetadataBuilder(PropertyMetadata<TEntity, object> propertyMetadata)
        {
            _propertyMetadata = propertyMetadata;
        }

        public PropertyMetadataBuilder<TEntity> Name(string value)
        {
            _propertyMetadata.Name = value;

            return this;
        }

        public PropertyMetadataBuilder<TEntity> MapFrom(string value)
        {
            _propertyMetadata.MapFrom = value;

            return this;
        }

        public PropertyMetadataBuilder<TEntity> Ignore()
        {
            _propertyMetadata.IsIgnore = true;

            return this;
        }

        public PropertyMetadataBuilder<TEntity> OrderBy()
        {
            _propertyMetadata.OrderDirection = OrderDirection.Ascending;

            return this;
        }

        public PropertyMetadataBuilder<TEntity> OrderByDescending()
        {
            _propertyMetadata.OrderDirection = OrderDirection.Descending;

            return this;
        }
    }
}
