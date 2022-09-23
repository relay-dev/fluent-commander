namespace FluentCommander
{
    /// <summary>
    /// Creates <see cref="IDatabaseRequestHandler"/> instances, using the default (when no name is specified) or the database instance by name
    /// </summary>
    public interface IDatabaseRequestHandlerFactory
    {
        /// <summary>
        /// Creates a new IDatabaseRequestHandler instance
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the IConnectionStringCollection</param>
        /// <returns>A new IDatabaseRequestHandler instance</returns>
        IDatabaseRequestHandler Create(string connectionStringName = "DefaultConnection");
    }
}