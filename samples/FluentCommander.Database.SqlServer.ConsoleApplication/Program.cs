using ConsoleApplication.SqlServer.Framework;
using ConsoleApplication.SqlServer.Samples;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication.SqlServer
{
    class Program
    {
        static void Main()
        {
            // Build configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            // Generate the IServiceProvider
            IServiceProvider serviceProvider = new Startup(config).ConfigureServices();

            // Initialize the database
            Setup(config);

            try
            {
                // Get the configured sample fixtures
                List<SampleFixture> sampleFixtures = GetSampleFixtures();

                // Run the program
                new SampleProgram(serviceProvider).Run(sampleFixtures);
            }
            finally
            {
                Teardown(config);
            }
        }

        private static void Setup(IConfiguration config)
        {
            new DatabaseService(config).SetupDatabase();
        }

        private static void Teardown(IConfiguration config)
        {
            new DatabaseService(config).TeardownDatabase();
        }

        private static List<SampleFixture> GetSampleFixtures()
        {
            return new List<SampleFixture>
            {
                new SampleFixture("1", "Basic Commands", typeof(BasicCommandSamples)),
                new SampleFixture("2", "Bulk Copy Commands", typeof(BulkCopyCommandSamples)),
                new SampleFixture("3", "Pagination Commands", typeof(PaginationCommandSamples)),
                new SampleFixture("4", "SQL Query Commands", typeof(SqlQueryCommandSamples)),
                new SampleFixture("5", "SQL Non-Query Commands", typeof(SqlNonQueryCommandSamples)),
                new SampleFixture("6", "Stored Procedure Commands", typeof(StoredProcedureCommandSamples)),
            };
        }
    }
}
