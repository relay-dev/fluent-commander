using FluentCommander.Samples.Setup;
using System.Data;
using System.Text.Json;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests
{
    public abstract class UnitTest
    {
        private readonly ITestOutputHelper _output;
        protected string TestUsername;

        protected UnitTest(ITestOutputHelper output)
        {
            _output = output;
            TestUsername = GetType().Name;
        }

        protected virtual void WriteLine(string s)
        {
            _output.WriteLine(s);
        }

        protected virtual void WriteLine(string s, params object[] args)
        {
            _output.WriteLine(s, args);
        }

        protected virtual void WriteLine(DataTable d)
        {
            _output.WriteLine(DatabaseService.Print(d));
        }

        protected virtual void WriteLine(object o)
        {
            _output.WriteLine(JsonSerializer.Serialize(o));
        }
    }
}
