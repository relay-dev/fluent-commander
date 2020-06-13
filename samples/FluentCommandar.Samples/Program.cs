using FluentCommander.Samples.Setup;
using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FluentCommander.Samples
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

            // Generate the Service Provider
            IServiceProvider serviceProvider = new Startup(config).ConfigureServices();

            try
            {
                // Initialize the database
                Setup(config);

                // Run the program
                await new SampleProgram(serviceProvider).Run();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("Encountered unhandled exception:");
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
