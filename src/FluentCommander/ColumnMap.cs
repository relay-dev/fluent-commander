﻿namespace FluentCommander
{
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
