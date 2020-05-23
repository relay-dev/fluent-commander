using ConsoleApplication.SqlServer.Framework;
using Core.Plugins.Extensions;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Samples
{
    /// <notes>
    /// This Sample class demonstrates how to build a stored procedure command using various combinations of input, output and return parameters
    /// To see the bodies of these Stored Procedures, navigate to the Resources folder and review the setup-*.sql files
    /// </notes>
    public class StoredProcedureCommandSamples : CommandSampleBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public StoredProcedureCommandSamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// Stored Procedures can be called with various input parameter types. This stored procedure has output, which is found on the result object
        /// </notes>
        private async Task ExecuteStoredProcedureWithAllInputTypesAndTableResult()
        {
            StoredProcedureCommandResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_AllInputTypes_NoOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleInt", 0)
                .AddInputParameter("SampleSmallInt", 0)
                .AddInputParameter("SampleTinyInt", 0)
                .AddInputParameter("SampleBit", 0)
                .AddInputParameter("SampleDecimal", 0)
                .AddInputParameter("SampleFloat", 0)
                .AddInputParameter("SampleDateTime", DateTime.Now)
                .AddInputParameter("SampleUniqueIdentifier", Guid.NewGuid())
                .AddInputParameter("SampleVarChar", "Row 1")
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Row count: {0}", result.Count);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Stored Procedures with output parameters need to call AddOutputParameter(), and retrieve the output from result.OutputParameters
        /// </notes>
        private async Task ExecuteStoredProcedureWithOutput()
        {
            string outputParameterName = "SampleOutputInt";

            StoredProcedureCommandResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntOutput_NoResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName, DbType.Int32)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
        }

        /// <notes>
        /// Stored Procedures with more complex signatures can be called. This stored procedure has input, output and it returns a DataTable with the result
        /// </notes>
        private async Task ExecuteStoredProcedureWithInputMultipleOutputAndTableResult()
        {
            string outputParameterName1 = "SampleOutputInt";
            string outputParameterName2 = "SampleOutputVarChar";

            StoredProcedureCommandResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_MultipleOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName1, DbType.Int32)
                .AddOutputParameter(outputParameterName2, DbType.String, 1000)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<int>(outputParameterName1));
            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<string>(outputParameterName2));
            Console.WriteLine("Row count: {0}", result.Count);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Stored Procedures with InputOutput parameters need to call AddInputOutputParameter(), and retrieve the output from result.OutputParameters
        /// </notes>
        private async Task ExecuteStoredProcedureWithInputOutputParameter()
        {
            string outputParameterName = "SampleInputOutputInt";

            StoredProcedureCommandResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntInputOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(outputParameterName, 1)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
            Console.WriteLine("Row count: {0}", result.Count);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Stored Procedures with InputOutput parameters need to call AddInputOutputParameter(), and retrieve the output from result.OutputParameters
        /// </notes>
        private async Task ExecuteStoredProcedureWithInputOutputParameterSpecifyingType()
        {
            string outputParameterName = "SampleInputOutputVarChar";

            StoredProcedureCommandResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_VarCharOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(outputParameterName, 1, DbType.String, 50)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<string>(outputParameterName));
            Console.WriteLine("Row count: {0}", result.Count);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Stored Procedures with Return parameters can retrieve them from result.ReturnParameters
        /// </notes>
        private async Task ExecuteStoredProcedureWithReturnParameter()
        {
            StoredProcedureCommandResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_NoInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Return parameter: {0}", result.GetReturnParameter<int>());
        }

        /// <notes>
        /// Stored Procedures with both Output parameters and a Return can be called from the command builder
        /// </notes>
        private async Task ExecuteStoredProcedureWithInputOutputAndReturn()
        {
            string outputParameterName = "SampleOutputBigInt";

            StoredProcedureCommandResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_Input_Output_ReturnBigInt]")
                .AddInputParameter("SampleTableID", 0)
                .AddInputOutputParameter(outputParameterName, 2)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
            Console.WriteLine("Return parameter: {0}", result.GetReturnParameter<int>());
        }

        /// <notes>
        /// Stored Procedures with optional parameters can be called
        /// </notes>
        private async Task ExecuteStoredProcedureWithOptionalInputParameter()
        {
            StoredProcedureCommandResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_OptionalInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Return parameter: {0}", result.GetReturnParameter<int>());
        }

        /// <notes>
        /// Input parameters require a database type parameter, which can often be inferred by looking at the type of the parameter value
        /// If that default behavior does not meet your needs, you can specify the database type of your input parameter using this variation of AddInputParameter()
        /// </notes>
        private async Task ExecuteStoredProcedureWithInputSpecifyingType()
        {
            StoredProcedureCommandResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_OptionalInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1, DbType.Int64)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Row count: {0}", result.Count);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        protected override void Init()
        {
            SampleMethods = new List<SampleMethodAsync>
            {
                new SampleMethodAsync("1", "ExecuteStoredProcedureWithAllInputTypesAndTableResult()", async () => await ExecuteStoredProcedureWithAllInputTypesAndTableResult()),
                new SampleMethodAsync("2", "ExecuteStoredProcedureWithOutput()", async () => await ExecuteStoredProcedureWithOutput()),
                new SampleMethodAsync("3", "ExecuteStoredProcedureWithInputMultipleOutputAndTableResult()", async () => await ExecuteStoredProcedureWithInputMultipleOutputAndTableResult()),
                new SampleMethodAsync("4", "ExecuteStoredProcedureWithInputOutputParameter()", async () => await ExecuteStoredProcedureWithInputOutputParameter()),
                new SampleMethodAsync("5", "ExecuteStoredProcedureWithInputOutputParameterSpecifyingType()", async () => await ExecuteStoredProcedureWithInputOutputParameterSpecifyingType()),
                new SampleMethodAsync("6", "ExecuteStoredProcedureWithReturnParameter()", async () => await ExecuteStoredProcedureWithReturnParameter()),
                new SampleMethodAsync("7", "ExecuteStoredProcedureWithInputOutputAndReturn()", async () => await ExecuteStoredProcedureWithInputOutputAndReturn()),
                new SampleMethodAsync("8", "ExecuteStoredProcedureWithOptionalInputParameter()", async () => await ExecuteStoredProcedureWithOptionalInputParameter()),
                new SampleMethodAsync("9", "ExecuteStoredProcedureWithInputSpecifyingType()", async () => await ExecuteStoredProcedureWithInputSpecifyingType())
            };
        }
    }
}
