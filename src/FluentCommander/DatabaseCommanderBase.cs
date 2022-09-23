using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander
{
    public abstract class DatabaseCommanderBase : IDatabaseCommander
    {
        protected readonly DatabaseCommandBuilder DatabaseCommandBuilder;

        protected DatabaseCommanderBase(DatabaseCommandBuilder databaseCommandBuilder)
        {
            DatabaseCommandBuilder = databaseCommandBuilder;
        }

        /// <summary>
        /// Initiates a database command
        /// </summary>
        /// <returns>A builder object that defines which command should be run</returns>
        public DatabaseCommandBuilder BuildCommand()
        {
            return DatabaseCommandBuilder;
        }

        /// <summary>
        /// Gets the server name this <see cref="IDatabaseCommander"/> instance is connected to
        /// </summary>
        /// <returns>The server name</returns>
        public abstract string GetServerName();

        /// <summary>
        /// Gets the server name this <see cref="IDatabaseCommander"/> instance is connected to
        /// </summary>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The server name</returns>
        public abstract Task<string> GetServerNameAsync(CancellationToken cancellationToken);
    }
}
