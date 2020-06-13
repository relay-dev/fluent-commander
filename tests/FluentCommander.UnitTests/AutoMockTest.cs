using FluentCommander.Samples.Setup;
using Moq.AutoMock;
using System.Data;
using System.Text.Json;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests
{
    public abstract class AutoMockTest<TCUT> where TCUT : class
    {
        private readonly ITestOutputHelper _output;
        protected readonly AutoMocker AutoMocker;
        protected string TestUsername;
        protected TCUT CUT => AutoMocker.CreateInstance<TCUT>();

        protected AutoMockTest(ITestOutputHelper output)
        {
            _output = output;
            AutoMocker = new AutoMocker();
            TestUsername = "AutoMockTest";
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
            _output.WriteLine(AutoMocker.CreateInstance<DatabaseService>().Print(d));
        }

        protected virtual void WriteLine(object o)
        {
            _output.WriteLine(JsonSerializer.Serialize(o));
        }
    }
}
