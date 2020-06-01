using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentCommander.Core.Property;

namespace FluentCommander.Core.Utility.Impl
{
    public class ReflectionUtility
    {
        public static List<TEntity> DataTableToList<TEntity>(DataTable dataTable, PropertyMapBuilder<TEntity> builder)
        {
            var targetList = dataTable.AsEnumerable().Select(dataRow =>
            {
                TEntity entity = Activator.CreateInstance<TEntity>();

                foreach (PropertyMetadata<TEntity, object> propertyMetadata in builder.PropertyMap.PropertyMetadata.Where(pm => !pm.IsIgnore))
                {
                    var memberSelectorExpression = GetMemberExpression(propertyMetadata.Selector);

                    if (memberSelectorExpression != null)
                    {
                        var property = memberSelectorExpression.Member as PropertyInfo;

                        if (property != null)
                        {
                            if (string.IsNullOrEmpty(propertyMetadata.MapFrom))
                            {
                                throw new InvalidOperationException("MapFrom was not set");
                            }

                            if (!dataRow.Table.Columns.Contains(propertyMetadata.MapFrom))
                            {
                                throw new InvalidOperationException($"Projection mapping is setup to anticipate the result containing a column named '{propertyMetadata.MapFrom}' which was not found");
                            }

                            property.SetValue(entity, dataRow[propertyMetadata.MapFrom], null);
                        }
                    }
                }

                return entity;
            }).ToList();

            return targetList;
        }

        public static List<T> DataTableToList<T>(DataTable dataTable)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            var columnNames = dataTable.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var objectProperties = typeof(T).GetProperties(flags);

            var targetList = dataTable.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }

                return instanceOfT;
            }).ToList();

            return targetList;
        }

        private static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> exp)
        {
            var member = exp.Body as MemberExpression;
            var unary = exp.Body as UnaryExpression;

            return member ?? unary?.Operand as MemberExpression;
        }
    }
}
