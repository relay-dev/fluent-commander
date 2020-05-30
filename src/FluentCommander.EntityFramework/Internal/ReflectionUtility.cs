using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace FluentCommander.EntityFramework.Internal
{
    public class ReflectionUtility
    {
        public static List<T> DataTableToList<T>(DataTable dataTable, PropertyMapBuilder<T> mapping)
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
    }
}
