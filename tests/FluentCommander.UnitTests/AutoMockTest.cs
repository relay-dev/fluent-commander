using Moq.AutoMock;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests
{
    public abstract class AutoMockTest : UnitTest
    {
        protected readonly AutoMocker AutoMocker;

        protected AutoMockTest(ITestOutputHelper output)
            : base(output)
        {
            AutoMocker = new AutoMocker();
        }
    }

    public abstract class AutoMockTest<TCUT> : AutoMockTest where TCUT : class
    {
        protected TCUT CUT => AutoMocker.CreateInstance<TCUT>();

        protected AutoMockTest(ITestOutputHelper output)
            : base(output) { }
    }
}
