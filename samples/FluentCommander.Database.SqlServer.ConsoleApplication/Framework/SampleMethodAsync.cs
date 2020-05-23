using System;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Framework
{
    public class SampleMethodAsync
    {
        public string Key { get; }
        public string Name { get; }
        public Func<Task> Action { get; }

        public SampleMethodAsync(string key, string name, Func<Task> action)
        {
            Key = key;
            Name = name;
            Action = action;
        }
    }
}
