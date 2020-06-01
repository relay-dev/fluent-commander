namespace FluentCommander.Core.Property
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

        public PropertyMetadataBuilder<TEntity> MapFrom(string value)
        {
            PropertyMetadata.MapFrom = value;

            return this;
        }

        public PropertyMetadataBuilder<TEntity> Ignore()
        {
            PropertyMetadata.IsIgnore = true;

            return this;
        }
    }
}
