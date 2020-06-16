using FluentCommander.SqlQuery;
using FluentCommander.SqlServer;
using FluentCommander.SqlServer.Internal;
using Microsoft.Data.SqlClient;
using Moq;
using Shouldly;
using System.Data;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests.FluentCommander.SqlServer.Commands
{
    public class SqlServerSqlQueryCommandTests : AutoMockTest
    {
        public SqlServerSqlQueryCommandTests(ITestOutputHelper output)
            : base(output) { }

        [Fact]
        public void SqlQueryCommand_ShouldReturnAsExpected_WhenDataTableIsNotNull()
        {
            // Arrange
            var input = new SqlQueryRequest();

            var connectionProviderMock = AutoMocker.GetMock<ISqlServerConnectionProvider>();
            var commandExecutorMock = AutoMocker.GetMock<ISqlServerCommandExecutor>();

            connectionProviderMock
                .Setup(mock => mock.GetConnection())
                .Returns(new SqlConnection("Server=(local)\\SQL2017;Integrated security=SSPI;"));

            commandExecutorMock
                .Setup(mock => mock.Execute(It.IsAny<SqlCommand>()))
                .Returns(new DataTable());

            var cut = new SqlServerSqlQueryCommand(connectionProviderMock.Object, commandExecutorMock.Object);

            // Act
            SqlQueryResult output = cut.Execute(input);

            // Assert
            output.ShouldNotBeNull();
        }
    }
}
