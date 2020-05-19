namespace FluentCommander.Database
{
    /// <summary>
    /// Creates <see cref="IDatabaseCommander"/> instances, using the default (when no name is specified) or the database instance by name
    /// </summary>
    public interface IDatabaseCommanderFactory
    {
        /// <summary>
        /// Creates a new IDatabaseCommander instance
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the IConnectionStringProvider</param>
        /// <returns>A new IDatabaseCommander instance</returns>
        IDatabaseCommander Create(string connectionStringName = null);
    }
}