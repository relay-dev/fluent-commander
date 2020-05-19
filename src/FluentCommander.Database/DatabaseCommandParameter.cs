using System.Data;

namespace FluentCommander.Database
{
    /// <summary>
    /// A generic abstraction of a database command parameter
    /// </summary>
    public class DatabaseCommandParameter
    {
        /// <summary>
        /// The name of the database parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value that should be passed as part of this Command
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// When the DbType is a string, this value represents the length of the string
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The type of the parameter that should be passed as part of this Command
        /// </summary>
        public DbType? DbType { get; set; }

        /// <summary>
        /// The direction of the parameter (Input, Output, InputOutput or Return)
        /// </summary>
        public ParameterDirection Direction { get; set; }
    }
}