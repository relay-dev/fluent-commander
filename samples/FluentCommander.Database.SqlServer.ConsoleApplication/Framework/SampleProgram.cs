using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Framework
{
    public class SampleProgram
    {
        private readonly IServiceProvider _serviceProvider;

        public SampleProgram(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Run(List<SampleFixture> sampleFixtures)
        {
            bool isExit = false;

            try
            {
                while (!isExit)
                {
                    DisplayMainMenu(sampleFixtures);

                    string selection = Console.ReadLine();

                    if (selection?.ToUpper() == "X")
                    {
                        isExit = true;
                    }
                    else
                    {
                        SampleFixture sample =
                            sampleFixtures.SingleOrDefault(s => string.Equals(s.Key, selection, StringComparison.OrdinalIgnoreCase));

                        if (sample == null)
                        {
                            Console.WriteLine("Invalid selection! Please try again (press any key to continue)");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            Console.Clear();

                            var sampleToRun = _serviceProvider.GetRequiredService(sample.SampleType);

                            if (!sampleToRun.GetType().GetInterfaces().Contains(typeof(ISample)) && !sampleToRun.GetType().GetInterfaces().Contains(typeof(ISampleAsync)))
                            {
                                Console.WriteLine("Error! That selection is not a Sample (press any key to continue)");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else if (sampleToRun.GetType().GetInterfaces().Contains(typeof(ISampleAsync)))
                            {
                                await ((ISampleAsync)sampleToRun).Run();
                            }
                            else
                            {
                                ((ISample)sampleToRun).Run();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine($"Encountered unhandled exception{Environment.NewLine}{Environment.NewLine}'{e.Message}'");
            }
        }

        private static void DisplayMainMenu(List<SampleFixture> sampleFixtures)
        {
            Console.WriteLine("Sample fixtures:{0}", Environment.NewLine);

            foreach (SampleFixture sampleFixture in sampleFixtures)
            {
                Console.WriteLine("{0}. {1}", sampleFixture.Key, sampleFixture.Name);
            }

            Console.WriteLine("{0}Select 1 - {1} and press Enter (select X and press Enter to exit)", Environment.NewLine, sampleFixtures.Max(s => s.Key));
        }
    }
}
