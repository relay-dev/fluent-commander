using System.Collections.Generic;

namespace FluentCommander.Core.Property
{
    public class PropertyMap<TEntity>
    {
        public List<PropertyMetadata<TEntity, object>> PropertyMetadataCollection { get; set; }

        public PropertyMap()
        {
            PropertyMetadataCollection = new List<PropertyMetadata<TEntity, object>>();
        }
    }
}
