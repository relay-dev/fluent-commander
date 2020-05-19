using System;

namespace ConsoleApplication.SqlServer.Framework
{
    public class SampleFixture
    {
        public string Key { get; }
        public string Name { get; }
        public Type SampleType { get; }

        public SampleFixture(string key, string name, Type sampleType)
        {
            Key = key;
            Name = name;
            SampleType = sampleType;
        }
    }
}
