using System;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Framework
{
    public class SampleMethodAsync
    {
        public string Key { get; }
        public string Name { get; }
        public Func<Task> Method { get; }

        public SampleMethodAsync(string key, string name, Func<Task> method)
        {
            Key = key;
            Name = name;
            Method = method;
        }
    }
}
