using System;
using System.Linq.Expressions;

namespace FluentCommander.Core.Property
{
    public class PropertyMetadata<TEntity, TProperty>
    {
        public Expression<Func<TEntity, TProperty>> Selector { get; set; }
        public string Name { get; set; }
        public string MapFrom { get; set; }
        public bool IsIgnore { get; set; }
    }
}
