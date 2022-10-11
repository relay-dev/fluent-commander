using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander
{
    /// <summary>
    /// The root-level type that provides access to all features in the library
    /// </summary>
    public interface IDatabaseCommander
    {
        /// <summary>
        /// Initiates a database command
        /// </summary>
        /// <returns>A builder object that defines which command should be run</returns>
        IDatabaseCommandBuilder BuildCommand();

        /// <summary>
        /// Gets the server name this <see cref="IDatabaseCommander"/> instance is connected to
        /// </summary>
        /// <returns>The server name</returns>
        string GetServerName();

        /// <summary>
        /// Gets the server name this <see cref="IDatabaseCommander"/> instance is connected to
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The server name</returns>
        Task<string> GetServerNameAsync(CancellationToken cancellationToken);
    }
}
