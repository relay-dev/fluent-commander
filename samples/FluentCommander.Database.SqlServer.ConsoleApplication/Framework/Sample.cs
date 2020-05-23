using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication.SqlServer.Framework
{
    public abstract class Sample : ISample
    {
        protected List<SampleMethod> SampleMethods;
        protected abstract void Init();

        public void Run()
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
                    SampleMethod sampleMethod = 
                        SampleMethods.SingleOrDefault(s => string.Equals(s.Key, selection, StringComparison.OrdinalIgnoreCase));

                    if (sampleMethod == null)
                    {
                        Console.WriteLine("Invalid selection! Please try again (press any key to continue)");
                    }
                    else
                    {
                        Console.Clear();

                        sampleMethod.Method.Invoke();

                        Console.WriteLine("{0}Press any key to continue...", Environment.NewLine);
                    }

                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        protected void DisplayMenu(List<SampleMethod> sampleMethods)
        {
            Console.WriteLine("Sample methods: {0}", Environment.NewLine);

            foreach (SampleMethod sampleMethod in sampleMethods)
            {
                Console.WriteLine("{0}. {1}", sampleMethod.Key, sampleMethod.Name);
            }

            Console.WriteLine("{0}Select 1{1} and press Enter (select M to return to the main menu)", Environment.NewLine, sampleMethods.Count == 1 ? string.Empty : " - " + sampleMethods.Max(sm => sm.Key));
        }
    }
}
