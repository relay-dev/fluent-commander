using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Framework
{
    public abstract class SampleAsync : ISampleAsync
    {
        public async Task Run()
        {
            bool isExit = false;

            List<SampleMethodAsync> sampleMethods = DiscoverSampleMethods();

            while (!isExit)
            {
                DisplayMenu(sampleMethods);

                string selection = Console.ReadLine();

                if (selection?.ToUpper() == "M")
                {
                    Console.Clear();
                    isExit = true;
                }
                else
                {
                    SampleMethodAsync sampleMethod =
                        sampleMethods.SingleOrDefault(s => string.Equals(s.Key, selection, StringComparison.OrdinalIgnoreCase));

                    if (sampleMethod == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid selection! Please try again (press any key to continue)");
                    }
                    else
                    {
                        Console.Clear();

                        await sampleMethod.Method.Invoke();

                        Console.WriteLine("{0}Press any key to continue...", Environment.NewLine);
                    }

                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private List<SampleMethodAsync> DiscoverSampleMethods()
        {
            List<MethodInfo> methodInfos = this.GetType().GetMethods()
                .Where(m => m.GetCustomAttribute<SampleAttribute>() != null)
                .OrderBy(mi => mi.Name)
                .ToList();

            var sampleMethods = new List<SampleMethodAsync>();

            for (int i = 0; i < methodInfos.Count; i++)
            {
                MethodInfo m = methodInfos[i];

                SampleAttribute sampleAttribute =
                    (SampleAttribute)m.GetCustomAttributes(typeof(SampleAttribute), true).Single();

                sampleMethods.Add(new SampleMethodAsync(sampleAttribute.Key ?? (i + 1).ToString(), sampleAttribute.Name ?? m.Name + "()", async () => await (Task)m.Invoke(this, null)));
            }

            return sampleMethods.OrderBy(sm => sm.Key).ToList();
        }

        protected void DisplayMenu(List<SampleMethodAsync> sampleMethods)
        {
            Console.WriteLine("Sample methods: {0}", Environment.NewLine);

            foreach (SampleMethodAsync sampleMethod in sampleMethods)
            {
                Console.WriteLine(" ({0}) {1}", sampleMethod.Key, sampleMethod.Name);
            }

            Console.WriteLine("{0}Select 1{1} and press Enter (select M to return to the main menu)", Environment.NewLine, sampleMethods.Count == 1 ? string.Empty : " - " + sampleMethods.Max(sm => sm.Key));
        }
    }
}
