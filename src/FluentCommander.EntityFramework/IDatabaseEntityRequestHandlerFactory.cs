namespace FluentCommander.EntityFramework
{
    /// <summary>
    /// Creates <see cref="IDatabaseEntityRequestHandler"/> instances, using the default (when no name is specified) or the database instance by name
    /// </summary>
    public interface IDatabaseEntityRequestHandlerFactory
    {
        /// <summary>
        /// Creates a new IDatabaseEntityRequestHandler instance
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the IConnectionStringCollection</param>
        /// <returns>A new IDatabaseEntityCommander instance</returns>
        IDatabaseEntityRequestHandler Create(string connectionStringName = null);
    }
}
