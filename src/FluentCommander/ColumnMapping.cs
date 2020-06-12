using System.Collections.Generic;

namespace FluentCommander
{
    public class ColumnMapping
    {
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
