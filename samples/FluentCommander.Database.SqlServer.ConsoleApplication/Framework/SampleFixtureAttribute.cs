using System;

namespace ConsoleApplication.SqlServer.Framework
{
    [AttributeUsageAttribute(AttributeTargets.Class)]
    public sealed class SampleFixtureAttribute : Attribute
    {
        /// <summary>
        /// The key of the sample that will display on the output window
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The key of the sample that will display on the output window
        /// </summary>
        public string Name { get; set; }
    }
}
