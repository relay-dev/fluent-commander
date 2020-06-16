using FluentCommander.BulkCopy;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests.BulkCopy
{
    public class BulkCopyCommandOptionsBuilderTests : UnitTest
    {
        public BulkCopyCommandOptionsBuilderTests(ITestOutputHelper output) 
            : base(output) { }

        [Fact]
        public void Options_ShouldNotBeNull_UnderAllConditions()
        {
            // Arrange
            var input = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder output = new BulkCopyCommandOptionsBuilder(input);

            // Assert
            output.ShouldNotBeNull();
            input.ShouldNotBeNull();
            input.Options.ShouldNotBeNull();

            // Print
            WriteLine(output);
        }

        [Fact]
        public void AllowEncryptedValueModifications_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var input = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder output = new BulkCopyCommandOptionsBuilder(input).AllowEncryptedValueModifications();

            // Assert
            output.ShouldNotBeNull();
            input.ShouldNotBeNull();
            input.Options.ShouldNotBeNull();
            input.Options.AllowEncryptedValueModifications.ShouldNotBeNull();
            input.Options.AllowEncryptedValueModifications.ShouldBe(true);

            // Print
            WriteLine(output);
        }

        [Fact]
        public void CheckConstraints_ShouldDisableTheFlag_WhenFlagIsFalse()
        {
            // Arrange
            var input = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder output = new BulkCopyCommandOptionsBuilder(input).CheckConstraints(false);

            // Assert
            output.ShouldNotBeNull();
            input.ShouldNotBeNull();
            input.Options.ShouldNotBeNull();
            input.Options.CheckConstraints.ShouldNotBeNull();
            input.Options.CheckConstraints.ShouldBe(false);

            // Print
            WriteLine(output);
        }

        [Fact]
        public void FireTriggers_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var input = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder output = new BulkCopyCommandOptionsBuilder(input).FireTriggers();

            // Assert
            output.ShouldNotBeNull();
            input.ShouldNotBeNull();
            input.Options.ShouldNotBeNull();
            input.Options.FireTriggers.ShouldNotBeNull();
            input.Options.FireTriggers.ShouldBe(true);

            // Print
            WriteLine(output);
        }

        [Fact]
        public void KeepIdentity_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var input = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder output = new BulkCopyCommandOptionsBuilder(input).KeepIdentity();

            // Assert
            output.ShouldNotBeNull();
            input.ShouldNotBeNull();
            input.Options.ShouldNotBeNull();
            input.Options.KeepIdentity.ShouldNotBeNull();
            input.Options.KeepIdentity.ShouldBe(true);

            // Print
            WriteLine(output);
        }

        [Fact]
        public void KeepNulls_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var input = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder output = new BulkCopyCommandOptionsBuilder(input).KeepNulls();

            // Assert
            output.ShouldNotBeNull();
            input.ShouldNotBeNull();
            input.Options.ShouldNotBeNull();
            input.Options.KeepNulls.ShouldNotBeNull();
            input.Options.KeepNulls.ShouldBe(true);

            // Print
            WriteLine(output);
        }

        [Fact]
        public void TableLock_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var input = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder output = new BulkCopyCommandOptionsBuilder(input).TableLock();

            // Assert
            output.ShouldNotBeNull();
            input.ShouldNotBeNull();
            input.Options.ShouldNotBeNull();
            input.Options.TableLock.ShouldNotBeNull();
            input.Options.TableLock.ShouldBe(true);

            // Print
            WriteLine(output);
        }

        [Fact]
        public void UseInternalTransaction_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var input = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder output = new BulkCopyCommandOptionsBuilder(input).UseInternalTransaction();

            // Assert
            output.ShouldNotBeNull();
            input.ShouldNotBeNull();
            input.Options.ShouldNotBeNull();
            input.Options.UseInternalTransaction.ShouldNotBeNull();
            input.Options.UseInternalTransaction.ShouldBe(true);

            // Print
            WriteLine(output);
        }
    }
}
