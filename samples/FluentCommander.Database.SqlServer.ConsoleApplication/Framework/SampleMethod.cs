using System;

namespace ConsoleApplication.SqlServer.Framework
{
    public class SampleMethod
    {
        public string Key { get; }
        public string Name { get; }
        public Action Action { get; }

        public SampleMethod(string key, string name, Action action)
        {
            Key = key;
            Name = name;
            Action = action;
        }
    }
}
