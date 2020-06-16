using Moq.AutoMock;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests
{
    public abstract class AutoMockTest<TCUT> : UnitTest where TCUT : class
    {
        protected readonly AutoMocker AutoMocker;
        protected TCUT CUT => AutoMocker.CreateInstance<TCUT>();

        protected AutoMockTest(ITestOutputHelper output)
            : base(output)
        {
            AutoMocker = new AutoMocker();
        }
    }
}
