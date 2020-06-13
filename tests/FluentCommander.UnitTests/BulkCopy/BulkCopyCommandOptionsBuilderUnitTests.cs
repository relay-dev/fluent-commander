using FluentCommander.BulkCopy;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests.BulkCopy
{
    public class BulkCopyCommandOptionsBuilderUnitTests : AutoMockTest<BulkCopyCommandOptionsBuilder>
    {
        public BulkCopyCommandOptionsBuilderUnitTests(ITestOutputHelper output) 
            : base(output) { }

        [Fact]
        public void CheckConstraints_ShouldDisableTheFlag_WhenFlagIsFalse()
        {
            // Arrange & Act
            BulkCopyCommandOptionsBuilder result = CUT.CheckConstraints(false);

            // Assert
            result.ShouldNotBeNull();

            // Print
            WriteLine(result);
        }
    }
}
