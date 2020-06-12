namespace FluentCommander.EntityFramework
{
    /// <summary>
    /// Creates <see cref="IDatabaseEntityCommander"/> instances, using the default (when no name is specified) or the database instance by name
    /// </summary>
    public interface IDatabaseEntityCommanderFactory
    {
        /// <summary>
        /// Creates a new IDatabaseCommander instance
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the IConnectionStringCollection</param>
        /// <returns>A new IDatabaseEntityCommander instance</returns>
        IDatabaseEntityCommander Create(string connectionStringName = null);
    }
}
