using System.Data;
using FluentCommander.Core.Options;
using FluentCommander.SqlServer;
using FluentCommander.SqlServer.Internal;
using FluentCommander.StoredProcedure;
using Microsoft.Data.SqlClient;
using Moq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests.FluentCommander.SqlServer.Commands
{
    public class SqlServerStoredProcedureCommandTests : AutoMockTest
    {
        public SqlServerStoredProcedureCommandTests(ITestOutputHelper output)
            : base(output) { }

        [Fact]
        public void StoredProcedureCommand_ShouldReturnAsExpected_WhenDataTableIsNotNull()
        {
            // Arrange
            var input = new StoredProcedureRequest();

            var connectionProviderMock = AutoMocker.GetMock<ISqlServerConnectionProvider>();
            var commandExecutorMock = AutoMocker.GetMock<ISqlServerCommandExecutor>();

            connectionProviderMock
                .Setup(mock => mock.GetConnection(new CommandOptions()))
                .Returns(new SqlConnection("Server=(local)\\SQL2017;Integrated security=SSPI;"));

            commandExecutorMock
                .Setup(mock => mock.Execute(It.IsAny<SqlCommand>()))
                .Returns(new DataTable());

            var cut = new SqlServerStoredProcedureCommand(connectionProviderMock.Object, commandExecutorMock.Object);
            
            // Act
            StoredProcedureResult output = cut.Execute(input);

            // Assert
            output.ShouldNotBeNull();
        }
    }
}
