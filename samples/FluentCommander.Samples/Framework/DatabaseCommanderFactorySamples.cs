using Consolater;
using FluentCommander.Samples.Commands;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Samples.Framework
{
    /// <summary>
    /// If your application needs to connect to multiple different databases, you can create instances of IDatabaseCommanders with specific database connection strings
    /// Specify the connection strings in the appsettings.json file, inject an instance of IDatabaseCommanderFactory, and reference the connection string name when calling IDatabaseCommanderFactory.Create()
    /// </summary>
    [ConsoleAppMenuItem]
    public class DatabaseCommanderFactorySamples : ConsoleAppBase
    {
        private readonly IDatabaseCommanderFactory _databaseCommanderFactory;

        public DatabaseCommanderFactorySamples(
            IDatabaseCommanderFactory databaseCommanderFactory,
            IConfiguration config)
            : base(config)
        {
            _databaseCommanderFactory = databaseCommanderFactory;
        }

        /// <notes>
        /// Creates an instance of an IDatabaseCommander connected to its data source using the connection string named AlternateConnectionString
        /// </notes>
        [ConsoleAppSelection(Key = "1")]
        public async Task DatabaseCommanderFactoryWorksWithAlternateConnectionStrings()
        {
            IDatabaseCommander databaseCommander = _databaseCommanderFactory.Create("AlternateConnectionString");

            string serverName = await databaseCommander.GetServerNameAsync(new CancellationToken());

            Console.WriteLine("Connected to: {0}", serverName);
        }
    }
}
