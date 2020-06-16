using FluentCommander.StoredProcedure;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FluentCommander.UnitTests.StoredProcedure
{
    public class StoredProcedureResultTests : UnitTest
    {
        public StoredProcedureResultTests(ITestOutputHelper output)
            : base(output) { }

        [Fact]
        public void StoredProcedureResult_ShouldHandleOutputParameters_WhenResultParametersAreNull()
        {
            // Arrange
            List<DatabaseCommandParameter> input = null;

            // Act
            var output = new StoredProcedureResult(new DataTable(), input);

            // Assert
            output.ShouldNotBeNull();
            output.OutputParameters.ShouldNotBeNull();
            output.OutputParameters.Count.ShouldBe(0);
        }

        [Fact]
        public void StoredProcedureResult_ShouldReturnOutputParameters_WhenResultParametersAreNotNull()
        {
            // Arrange
            var input = new List<DatabaseCommandParameter>
            {
                new DatabaseCommandParameter
                {
                    Name = "Testing",
                    Direction = ParameterDirection.Output
                }
            };

            // Act
            var output = new StoredProcedureResult(new DataTable(), input);

            // Assert
            output.ShouldNotBeNull();
            output.OutputParameters.ShouldNotBeNull();
            output.OutputParameters.Count.ShouldBe(1);
            output.OutputParameters.Single().Value.Name.ShouldBe("Testing");
        }

        [Fact]
        public void StoredProcedureResult_ShouldReturnInputOutputParameters_WhenResultParametersAreNotNull()
        {
            // Arrange
            var input = new List<DatabaseCommandParameter>
            {
                new DatabaseCommandParameter
                {
                    Name = "Testing",
                    Direction = ParameterDirection.InputOutput
                }
            };

            // Act
            var output = new StoredProcedureResult(new DataTable(), input);

            // Assert
            output.ShouldNotBeNull();
            output.OutputParameters.ShouldNotBeNull();
            output.OutputParameters.Count.ShouldBe(1);
            output.OutputParameters.Single().Value.Name.ShouldBe("Testing");
        }

        [Fact]
        public void StoredProcedureResult_ShouldNotReturnInputParametersAsOutputParameters_WhenResultParametersAreNotNull()
        {
            // Arrange
            var input = new List<DatabaseCommandParameter>
            {
                new DatabaseCommandParameter
                {
                    Name = "Testing",
                    Direction = ParameterDirection.Input
                }
            };

            // Act
            var output = new StoredProcedureResult(new DataTable(), input);

            // Assert
            output.ShouldNotBeNull();
            output.OutputParameters.ShouldNotBeNull();
            output.OutputParameters.Count.ShouldBe(0);
        }

        [Fact]
        public void GetOutputParameter_ShouldReturnInputOutputParameters_WhenResultParametersAreNotNull()
        {
            // Arrange
            var input = new List<DatabaseCommandParameter>
            {
                new DatabaseCommandParameter
                {
                    Name = "Testing",
                    DatabaseType = DbType.Int32.ToString(),
                    Value = 100,
                    Direction = ParameterDirection.InputOutput
                }
            };

            // Act
            var output = new StoredProcedureResult(new DataTable(), input);

            // Assert
            output.ShouldNotBeNull();
            output.GetOutputParameter<int>("Testing").ShouldBe(100);
        }

        [Fact]
        public void GetOutputParameter_ShouldThrowException_WhenParametersDoNotContainReturn()
        {
            // Arrange
            var input = new List<DatabaseCommandParameter>();

            // Act
            var output = new StoredProcedureResult(new DataTable(), input);

            // Assert
            output.ShouldNotBeNull();
            Assert.Throws<InvalidOperationException>(() => output.GetOutputParameter<int>("Testing"));
        }

        [Fact]
        public void StoredProcedureResult_ShouldReturnReturnParameters_WhenResultParametersAreNotNull()
        {
            // Arrange
            var input = new List<DatabaseCommandParameter>
            {
                new DatabaseCommandParameter
                {
                    Name = "Testing",
                    Direction = ParameterDirection.ReturnValue
                }
            };

            // Act
            var output = new StoredProcedureResult(new DataTable(), input);

            // Assert
            output.ShouldNotBeNull();
            output.ReturnParameter.ShouldNotBeNull();
            output.ReturnParameter.Name.ShouldBe("Testing");
        }

        [Fact]
        public void GetReturnParameter_ShouldReturnReturnParameters_WhenResultParametersAreNotNull()
        {
            // Arrange
            var input = new List<DatabaseCommandParameter>
            {
                new DatabaseCommandParameter
                {
                    Name = "Testing",
                    DatabaseType = DbType.Int32.ToString(),
                    Value = 100,
                    Direction = ParameterDirection.ReturnValue
                }
            };

            // Act
            var output = new StoredProcedureResult(new DataTable(), input);

            // Assert
            output.ShouldNotBeNull();
            output.GetReturnParameter<int>().ShouldBe(100);
        }

        [Fact]
        public void GetReturnParameter_ShouldThrowException_WhenParametersDoNotContainReturn()
        {
            // Arrange
            var input = new List<DatabaseCommandParameter>();

            // Act
            var output = new StoredProcedureResult(new DataTable(), input);

            // Assert
            output.ShouldNotBeNull();
            Assert.Throws<InvalidOperationException>(() => output.GetReturnParameter<int>());
        }
    }
}
