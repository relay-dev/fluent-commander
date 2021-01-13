using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FluentCommander.Core.Utility.Impl
{
    internal class AutoMapper : IAutoMapper
    {
        private readonly IDatabaseCommander _databaseCommander;

        public AutoMapper(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public void MapDataTableToTable(string tableName, DataTable dataTable, ColumnMapping columnMapping)
        {
            DataTable emptyDataTable = _databaseCommander.ExecuteSql($"SELECT * FROM {tableName} WHERE 1 = 0");

            List<string> databaseColumnNames = emptyDataTable.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName).ToList();
            List<string> fileColumnNames = dataTable.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName).ToList();

            // Ensure proper casing of source columns
            foreach (string fileColumnName in fileColumnNames)
            {
                ColumnMap columnMap = columnMapping.ColumnMaps.FirstOrDefault(cm => cm.Source.Equals(fileColumnName, StringComparison.InvariantCultureIgnoreCase));

                if (columnMap != null && columnMap.Source != fileColumnName)
                {
                    columnMap.Source = fileColumnName;
                }
            }

            // Ensure proper casing of destination columns
            foreach (string databaseColumnName in databaseColumnNames)
            {
                ColumnMap columnMap = columnMapping.ColumnMaps.FirstOrDefault(cm => cm.Destination.Equals(databaseColumnName, StringComparison.InvariantCultureIgnoreCase));

                if (columnMap != null && columnMap.Destination != databaseColumnName)
                {
                    columnMap.Destination = databaseColumnName;
                }
            }

            // Ensure existence of source columns
            foreach (string sourceColumnName in columnMapping.ColumnMaps.Select(cm => cm.Source))
            {
                if (!fileColumnNames.Contains(sourceColumnName))
                {
                    throw new InvalidOperationException($"The source column '{sourceColumnName}' does not exist");
                }
            }

            // Ensure existence of destination columns
            foreach (string destinationColumnName in columnMapping.ColumnMaps.Select(cm => cm.Destination))
            {
                if (!databaseColumnNames.Contains(destinationColumnName))
                {
                    throw new InvalidOperationException($"The destination column '{destinationColumnName}' does not exist");
                }
            }

            // AutoMap any columns that are not already mapped from the source to the destination
            foreach (string fileColumnName in fileColumnNames)
            {
                string databaseColumnName = databaseColumnNames.FirstOrDefault(s => s.Equals(fileColumnName, StringComparison.InvariantCultureIgnoreCase));

                if (databaseColumnName != null && columnMapping.ColumnMaps.All(cm => cm.Source != fileColumnName) && columnMapping.ColumnMaps.All(cm => cm.Destination != databaseColumnName))
                {
                    columnMapping.ColumnMaps.Add(new ColumnMap(fileColumnName, databaseColumnName));
                }
            }
        }
    }
}
