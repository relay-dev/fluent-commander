using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using System;
using System.IO;
using System.Threading.Tasks;
using Setup;

namespace ConsoleApplication.SqlServer
{
    class Program
    {
        static async Task Main()
        {
            // Build configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Generate the IServiceProvider
            IServiceProvider serviceProvider = new Startup(config).ConfigureServices();

            // Initialize the database
            Setup(config);

            try
            {
                // Run the program
                await new SampleProgram(serviceProvider).Run();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("Encountered unhandled exception");
                Console.WriteLine(e.Message);
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
    }
}
