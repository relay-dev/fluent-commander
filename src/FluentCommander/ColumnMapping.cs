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
    }

    public class ColumnMap
    {
        /// <summary>
        /// The source column
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The destination column
        /// </summary>
        public string Destination { get; set; }

        public ColumnMap() { }

        public ColumnMap(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}
