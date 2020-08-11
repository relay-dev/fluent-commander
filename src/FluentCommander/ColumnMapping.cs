using System.Collections.Generic;

namespace FluentCommander
{
    public class ColumnMapping
    {
        /// <summary>
        /// The collection of <see cref="ColumnMap"/> objects to define the mapping between the source and destination
        /// </summary>
        public List<ColumnMap> ColumnMaps;

        public ColumnMapping()
        {
            ColumnMaps = new List<ColumnMap>();
        }

        public ColumnMapping(List<ColumnMap> columnMaps)
        {
            ColumnMaps = columnMaps;
        }
    }
}
