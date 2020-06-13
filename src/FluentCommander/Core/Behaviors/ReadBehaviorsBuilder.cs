namespace FluentCommander.Core.Behaviors
{
    public class ReadBehaviorsBuilder
    {
        private readonly IHaveReadBehaviors _request;

        public ReadBehaviorsBuilder(IHaveReadBehaviors request)
        {
            _request = request;
            _request.ReadBehaviors ??= new ReadBehaviors();
        }

        /// <summary>
        /// When the command is executed, the associated Connection object is closed when the associated DataReader object is closed
        /// </summary>
        /// <param name="flag">Set to <c>true</c> if the given behavior is required; otherwise, <c>false</c></param>
        /// <returns>This instance of the builder</returns>
        public ReadBehaviorsBuilder CloseConnection(bool flag = true)
        {
            _request.ReadBehaviors.CloseConnection = flag;

            return this;
        }

        /// <summary>
        /// The query returns column and primary key information. The provider appends extra columns to the result set for existing primary key and timestamp columns
        /// </summary>
        /// <param name="flag">Set to <c>true</c> if the given behavior is required; otherwise, <c>false</c></param>
        /// <returns>This instance of the builder</returns>
        public ReadBehaviorsBuilder KeyInfo(bool flag = true)
        {
            _request.ReadBehaviors.KeyInfo = flag;

            return this;
        }

        /// <summary>
        /// Provides a way for the DataReader to handle rows that contain columns with large binary values. Rather than loading the entire row, SequentialAccess enables the DataReader to load data as a stream. You can then use the GetBytes or GetChars method to specify a byte location to start the read operation, and a limited buffer size for the data being returned
        /// </summary>
        /// <param name="flag">Set to <c>true</c> if the given behavior is required; otherwise, <c>false</c></param>
        /// <returns>This instance of the builder</returns>
        public ReadBehaviorsBuilder SequentialAccess(bool flag = true)
        {
            _request.ReadBehaviors.SequentialAccess = flag;

            return this;
        }

        /// <summary>
        /// The query returns column information only. When using SchemaOnly, the .NET Framework Data Provider for SQL Server precedes the statement being executed with SET FMTONLY ON
        /// </summary>
        /// <param name="flag">Set to <c>true</c> if the given behavior is required; otherwise, <c>false</c></param>
        /// <returns>This instance of the builder</returns>
        public ReadBehaviorsBuilder SchemaOnly(bool flag = true)
        {
            _request.ReadBehaviors.SchemaOnly = flag;

            return this;
        }

        /// <summary>
        /// The query returns a single result set
        /// </summary>
        /// <param name="flag">Set to <c>true</c> if the given behavior is required; otherwise, <c>false</c></param>
        /// <returns>This instance of the builder</returns>
        public ReadBehaviorsBuilder SingleResult(bool flag = true)
        {
            _request.ReadBehaviors.SingleResult = flag;

            return this;
        }

        /// <summary>
        /// The query is expected to return a single row of the first result set. Execution of the query may affect the database state. Some .NET Framework data providers may, but are not required to, use this information to optimize the performance of the command. When you specify SingleRow with the ExecuteReader() method of the OleDbCommand object, the .NET Framework Data Provider for OLE DB performs binding using the OLE DB IRow interface if it is available. Otherwise, it uses the IRowset interface. If your SQL statement is expected to return only a single row, specifying SingleRow can also improve application performance. It is possible to specify SingleRow when executing queries that are expected to return multiple result sets. In that case, where both a multi-result set SQL query and single row are specified, the result returned will contain only the first row of the first result set. The other result sets of the query will not be returned
        /// </summary>
        /// <param name="flag">Set to <c>true</c> if the given behavior is required; otherwise, <c>false</c></param>
        /// <returns>This instance of the builder</returns>
        public ReadBehaviorsBuilder SingleRow(bool flag = true)
        {
            _request.ReadBehaviors.SingleRow = flag;

            return this;
        }
    }
}
