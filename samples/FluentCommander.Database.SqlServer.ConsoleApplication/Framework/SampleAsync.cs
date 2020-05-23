using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Framework
{
    public abstract class SampleAsync : ISampleAsync
    {
        protected List<SampleMethodAsync> SampleMethods;
        protected abstract void Init();

        public async Task Run()
        {
            bool isExit = false;

            Init();

            while (!isExit)
            {
                DisplayMenu(SampleMethods);

                string selection = Console.ReadLine();

                if (selection?.ToUpper() == "M")
                {
                    Console.Clear();
                    isExit = true;
                }
                else
                {
                    SampleMethodAsync sampleMethod =
                        SampleMethods.SingleOrDefault(s => string.Equals(s.Key, selection, StringComparison.OrdinalIgnoreCase));

                    if (sampleMethod == null)
                    {
                        Console.WriteLine("Invalid selection! Please try again (press any key to continue)");
                    }
                    else
                    {
                        Console.Clear();

                        await sampleMethod.Action.Invoke();

                        Console.WriteLine("{0}Press any key to continue...", Environment.NewLine);
                    }

                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        protected void DisplayMenu(List<SampleMethodAsync> sampleMethods)
        {
            Console.WriteLine("Sample methods: {0}", Environment.NewLine);

            foreach (SampleMethodAsync sampleMethod in sampleMethods)
            {
                Console.WriteLine("{0}. {1}", sampleMethod.Key, sampleMethod.Name);
            }

            Console.WriteLine("{0}Select 1{1} and press Enter (select M to return to the main menu)", Environment.NewLine, sampleMethods.Count == 1 ? string.Empty : " - " + sampleMethods.Max(sm => sm.Key));
        }
    }
}
