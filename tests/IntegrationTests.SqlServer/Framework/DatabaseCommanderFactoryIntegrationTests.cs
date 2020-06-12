using FluentCommander;
using Shouldly;
using System;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.SqlServer.Framework
{
    [Collection("Service Provider collection")]
    public class DatabaseCommanderFactoryIntegrationTests : SqlServerIntegrationTest<IDatabaseCommanderFactory>
    {
        public DatabaseCommanderFactoryIntegrationTests(ServiceProviderFixture serviceProviderFixture, ITestOutputHelper output)
            : base(serviceProviderFixture, output) { }

        [Fact]
        public void DatabaseCommanderFactoryCreate_ShouldUseDefaultConnection_WhenNoConnectionNameIsSpecified()
        {
            // Arrange & Act
            string result = SUT.Create().GetServerName();

            // Assert
            result.ShouldNotBeNull();

            // Print
            WriteLine(result);
        }

        [Fact]
        public void DatabaseCommanderFactoryCreate_ShouldUseSpecificConnection_WhenConnectionNameIsSpecified()
        {
            // Arrange
            string result = null;
            const string connectionName = "InvalidConnectionString";

            // Act & Assert
            Should.Throw<ArgumentException>(() => result = SUT.Create(connectionName).GetServerName())
                .Message.ShouldBe("Format of the initialization string does not conform to specification starting at index 0.");

            // Assert
            result.ShouldBeNull();
        }
    }
}
