using System.Collections.Generic;

namespace FluentCommander.EntityFramework
{
    public class PropertyMap<TEntity>
    {
        public PropertyMap()
        {
            PropertyMetadata = new List<PropertyMetadata<TEntity, object>>();
        }

        public List<PropertyMetadata<TEntity, object>> PropertyMetadata { get; set; }
    }
}
