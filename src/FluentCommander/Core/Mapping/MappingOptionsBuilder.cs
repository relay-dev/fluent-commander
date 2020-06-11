using System;
using System.Linq;
using System.Linq.Expressions;
using FluentCommander.Core.Property;

namespace FluentCommander.Core.Mapping
{
    public class MappingOptionsBuilder
    {
        private readonly IColumnMappingRequest _request;

        public MappingOptionsBuilder(IColumnMappingRequest request)
        {
            _request = request;
        }

        public MappingOptionsBuilder UseAutoMap()
        {
            _request.MappingType = MappingType.AutoMap;

            return this;
        }

        public MappingOptionsBuilder UsePartialMap(ColumnMapping columnMapping)
        {
            _request.MappingType = MappingType.PartialMap;
            _request.ColumnMapping = columnMapping;

            return this;
        }

        public MappingOptionsBuilder UseMap(ColumnMapping columnMapping)
        {
            _request.MappingType = MappingType.ManualMap;
            _request.ColumnMapping = columnMapping;

            return this;
        }
    }

    public class MappingOptionsBuilder<TEntity>
    {
        private readonly IColumnMappingRequest _request;

        public MappingOptionsBuilder(IColumnMappingRequest request)
        {
            _request = request;
        }

        public MappingOptionsBuilder<TEntity> UseAutoMap()
        {
            _request.MappingType = MappingType.AutoMap;

            return this;
        }

        public MappingOptionsBuilder<TEntity> UsePartialMap(Action<PropertyMapBuilder<TEntity>> mappingBuilder)
        {
            _request.MappingType = MappingType.PartialMap;
            _request.ColumnMapping = ParseToColumnMapping(mappingBuilder);

            return this;
        }

        public MappingOptionsBuilder<TEntity> UseMap(Action<PropertyMapBuilder<TEntity>> mappingBuilder)
        {
            _request.MappingType = MappingType.ManualMap;
            _request.ColumnMapping = ParseToColumnMapping(mappingBuilder);

            return this;
        }

        private ColumnMapping ParseToColumnMapping(Action<PropertyMapBuilder<TEntity>> mappingBuilder)
        {
            var columnMapping = new ColumnMapping();

            var options = new PropertyMapBuilder<TEntity>();

            mappingBuilder(options);

            foreach (PropertyMetadata<TEntity, object> property in options.PropertyMap.PropertyMetadataCollection.Where(p => !p.IsIgnore))
            {
                var member = property.Selector.Body as MemberExpression;
                var unary = property.Selector.Body as UnaryExpression;

                string name = ((MemberExpression)(member ?? unary?.Operand))?.Member.Name;

                columnMapping.ColumnMaps.Add(
                    new ColumnMap
                    {
                        Source = property.MapFrom,
                        Destination = name
                    });
            }

            return columnMapping;
        }
    }
}
