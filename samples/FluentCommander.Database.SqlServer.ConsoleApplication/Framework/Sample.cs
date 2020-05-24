using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleApplication.SqlServer.Framework
{
    public abstract class Sample : ISample
    {
        public void Run()
        {
            bool isExit = false;

            List<SampleMethod> sampleMethods = DiscoverSampleMethods();

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
                    SampleMethod sampleMethod =
                        sampleMethods.SingleOrDefault(s => string.Equals(s.Key, selection, StringComparison.OrdinalIgnoreCase));

                    if (sampleMethod == null)
                    {
                        Console.Clear();
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

        private List<SampleMethod> ResolveSampleMethods(MethodInfo[] methodInfos)
        {
            SampleAttribute[] sampleAttributes = methodInfos
                .OrderBy(mi => mi.Name)
                .Cast<SampleAttribute>()
                .ToArray();

            var sampleMethods = new List<SampleMethod>();

            for (int i = 0; i < methodInfos.Length; i++)
            {
                SampleAttribute sa = sampleAttributes[i];
                MethodInfo m = methodInfos.Single(mi => mi.Name == sa.Name);

                sampleMethods.Add(new SampleMethod(sa.Key ?? i.ToString(), sa.Name ?? m.Name + "()", () => m.Invoke(this, null)));
            }

            return sampleMethods;
        }

        private List<SampleMethod> DiscoverSampleMethods()
        {
            List<MethodInfo> methodInfos = this.GetType().GetMethods()
                .Where(m => m.GetCustomAttribute<SampleAttribute>() != null)
                .OrderBy(mi => mi.Name)
                .ToList();

            var sampleMethods = new List<SampleMethod>();

            for (int i = 0; i < methodInfos.Count; i++)
            {
                MethodInfo m = methodInfos[i];

                SampleAttribute sampleAttribute =
                    (SampleAttribute)m.GetCustomAttributes(typeof(SampleAttribute), true).Single();

                sampleMethods.Add(new SampleMethod(sampleAttribute.Key ?? (i + 1).ToString(), sampleAttribute.Name ?? m.Name + "()", () => m.Invoke(this, null)));
            }

            return sampleMethods.OrderBy(sm => sm.Key).ToList();
        }

        protected void DisplayMenu(List<SampleMethod> sampleMethods)
        {
            Console.WriteLine("Sample methods: {0}", Environment.NewLine);

            foreach (SampleMethod sampleMethod in sampleMethods)
            {
                Console.WriteLine(" ({0}) {1}", sampleMethod.Key, sampleMethod.Name);
            }

            Console.WriteLine("{0}Select {1}{2} and press Enter (select M to return to the main menu)", Environment.NewLine, sampleMethods.Min(s => s.Key), sampleMethods.Count == 1 ? string.Empty : " - " + sampleMethods.Max(sm => sm.Key));
        }
    }
}
