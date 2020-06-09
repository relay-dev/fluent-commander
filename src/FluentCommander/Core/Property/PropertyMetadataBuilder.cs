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
    }
}
