using System;

namespace ConsoleApplication.SqlServer.Framework
{
    public class SampleMethod
    {
        public string Key { get; }
        public string Name { get; }
        public Action Method { get; }

        public SampleMethod(string key, string name, Action method)
        {
            Key = key;
            Name = name;
            Method = method;
        }
    }
}
