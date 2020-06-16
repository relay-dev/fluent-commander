using FluentCommander.BulkCopy;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests.BulkCopy
{
    public class BulkCopyCommandOptionsBuilderUnitTests : UnitTest
    {
        public BulkCopyCommandOptionsBuilderUnitTests(ITestOutputHelper output) 
            : base(output) { }

        [Fact]
        public void Options_ShouldNotBeNull_UnderAllConditions()
        {
            // Arrange
            var request = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder result = new BulkCopyCommandOptionsBuilder(request);

            // Assert
            result.ShouldNotBeNull();
            request.ShouldNotBeNull();
            request.Options.ShouldNotBeNull();

            // Print
            WriteLine(result);
        }

        [Fact]
        public void AllowEncryptedValueModifications_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var request = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder result = new BulkCopyCommandOptionsBuilder(request).AllowEncryptedValueModifications();

            // Assert
            result.ShouldNotBeNull();
            request.ShouldNotBeNull();
            request.Options.ShouldNotBeNull();
            request.Options.AllowEncryptedValueModifications.ShouldNotBeNull();
            request.Options.AllowEncryptedValueModifications.ShouldBe(true);

            // Print
            WriteLine(result);
        }

        [Fact]
        public void CheckConstraints_ShouldDisableTheFlag_WhenFlagIsFalse()
        {
            // Arrange
            var request = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder result = new BulkCopyCommandOptionsBuilder(request).CheckConstraints(false);

            // Assert
            result.ShouldNotBeNull();
            request.ShouldNotBeNull();
            request.Options.ShouldNotBeNull();
            request.Options.CheckConstraints.ShouldNotBeNull();
            request.Options.CheckConstraints.ShouldBe(false);

            // Print
            WriteLine(result);
        }

        [Fact]
        public void FireTriggers_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var request = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder result = new BulkCopyCommandOptionsBuilder(request).FireTriggers();

            // Assert
            result.ShouldNotBeNull();
            request.ShouldNotBeNull();
            request.Options.ShouldNotBeNull();
            request.Options.FireTriggers.ShouldNotBeNull();
            request.Options.FireTriggers.ShouldBe(true);

            // Print
            WriteLine(result);
        }

        [Fact]
        public void KeepIdentity_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var request = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder result = new BulkCopyCommandOptionsBuilder(request).KeepIdentity();

            // Assert
            result.ShouldNotBeNull();
            request.ShouldNotBeNull();
            request.Options.ShouldNotBeNull();
            request.Options.KeepIdentity.ShouldNotBeNull();
            request.Options.KeepIdentity.ShouldBe(true);

            // Print
            WriteLine(result);
        }

        [Fact]
        public void KeepNulls_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var request = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder result = new BulkCopyCommandOptionsBuilder(request).KeepNulls();

            // Assert
            result.ShouldNotBeNull();
            request.ShouldNotBeNull();
            request.Options.ShouldNotBeNull();
            request.Options.KeepNulls.ShouldNotBeNull();
            request.Options.KeepNulls.ShouldBe(true);

            // Print
            WriteLine(result);
        }

        [Fact]
        public void TableLock_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var request = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder result = new BulkCopyCommandOptionsBuilder(request).TableLock();

            // Assert
            result.ShouldNotBeNull();
            request.ShouldNotBeNull();
            request.Options.ShouldNotBeNull();
            request.Options.TableLock.ShouldNotBeNull();
            request.Options.TableLock.ShouldBe(true);

            // Print
            WriteLine(result);
        }

        [Fact]
        public void UseInternalTransaction_ShouldSetTheFlag_WhenFlagIsTrue()
        {
            // Arrange
            var request = new BulkCopyRequest();

            // Act
            BulkCopyCommandOptionsBuilder result = new BulkCopyCommandOptionsBuilder(request).UseInternalTransaction();

            // Assert
            result.ShouldNotBeNull();
            request.ShouldNotBeNull();
            request.Options.ShouldNotBeNull();
            request.Options.UseInternalTransaction.ShouldNotBeNull();
            request.Options.UseInternalTransaction.ShouldBe(true);

            // Print
            WriteLine(result);
        }
    }
}
