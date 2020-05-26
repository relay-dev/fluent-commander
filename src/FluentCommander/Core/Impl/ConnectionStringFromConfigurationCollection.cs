using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace FluentCommander.Core.Impl
{
    public class ConnectionStringFromConfigurationCollection : IConnectionStringCollection
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringFromConfigurationCollection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Get(string connectionStringName)
        {
            string connectionString = _configuration.GetConnectionString(connectionStringName);

            if (connectionString == null)
                throw new Exception($"No connection string with name '{connectionStringName}' could be found");

            if (connectionString == string.Empty)
                throw new Exception($"Connection string with name '{connectionStringName}' cannot be an empty string");

            if (connectionString.Contains("{{"))
            {
                Dictionary<string, string> connectionStringPlaceholders = ParsePlaceholders(connectionString);

                foreach (KeyValuePair<string, string> connectionStringPlaceholder in connectionStringPlaceholders)
                {
                    if (connectionString.Contains(connectionStringPlaceholder.Key))
                    {
                        connectionString = connectionString.Replace(connectionStringPlaceholder.Key, _configuration[connectionStringPlaceholder.Value]);
                    }
                }
            }

            return connectionString;
        }

        public List<string> ConnectionStringNames
        {
            get
            {
                return _configuration.GetSection("ConnectionStrings").GetChildren().Select(s => s.Key).ToList();
            }
        }

        private Dictionary<string, string> ParsePlaceholders(string connectionString)
        {
            var regex = new Regex(@"{{(.*?)}}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Dictionary<string, string> connectionStringVariables = regex.Matches(connectionString)
                .Cast<IEnumerable<Match>>()
                .Select(match => match.ToString())
                .OrderBy(s => s)
                .ToDictionary(s => s, s => s.Replace("{{", string.Empty).Replace("}}", string.Empty));

            return connectionStringVariables;
        }
    }
}
