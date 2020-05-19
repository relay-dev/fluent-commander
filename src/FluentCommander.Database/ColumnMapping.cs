using System.Collections.Generic;

namespace FluentCommander.Database
{
    public class ColumnMapping
    {
        public List<ColumnMap> ColumnMaps;

        public ColumnMapping()
        {
            ColumnMaps = new List<ColumnMap>();
        }
    }

    public class ColumnMap
    {
        public string Source { get; set; }
        public string Destination { get; set; }

        public ColumnMap() { }

        public ColumnMap(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}
