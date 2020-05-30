using System;
using System.Linq.Expressions;

namespace FluentCommander.EntityFramework
{
    public class PropertyMetadata<TEntity, TProperty>
    {
        public Expression<Func<TEntity, TProperty>> Selector { get; set; }
        public string Name { get; set; }
        public string MapFrom { get; set; }
        public bool IsKey { get; set; }
        public bool IsIgnored { get; set; }
    }
}
