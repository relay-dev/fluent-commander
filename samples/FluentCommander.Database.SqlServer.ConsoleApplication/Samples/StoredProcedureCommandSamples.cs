using ConsoleApplication.SqlServer.Framework;
using Core.Plugins.Extensions;
using FluentCommander.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

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
        private void ExecuteStoredProcedureWithAllInputTypesAndTableResult()
        {
            StoredProcedureCommandResult result = _databaseCommander.BuildCommand()
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
                .Execute();

            Console.WriteLine("Row count: {0}", result.DataTable.Rows);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Stored Procedures with output parameters need to call AddOutputParameter(), and retrieve the output from result.OutputParameters
        /// </notes>
        private void ExecuteStoredProcedureWithOutput()
        {
            string outputParameterName = "SampleOutputInt";

            StoredProcedureCommandResult result = _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntOutput_NoResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName, DbType.Int32)
                .Execute();

            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
        }

        /// <notes>
        /// Stored Procedures with more complex signatures can be called. This stored procedure has input, output and it returns a DataTable with the result
        /// </notes>
        private void ExecuteStoredProcedureWithInputMultipleOutputAndTableResult()
        {
            string outputParameterName1 = "SampleOutputInt";
            string outputParameterName2 = "SampleOutputVarChar";

            StoredProcedureCommandResult result = _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_MultipleOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName1, DbType.Int32)
                .AddOutputParameter(outputParameterName2, DbType.String, 1000)
                .Execute();

            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<int>(outputParameterName1));
            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<string>(outputParameterName2));
            Console.WriteLine("Row count: {0}", result.DataTable.Rows);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Stored Procedures with InputOutput parameters need to call AddInputOutputParameter(), and retrieve the output from result.OutputParameters
        /// </notes>
        private void ExecuteStoredProcedureWithInputOutputParameter()
        {
            string outputParameterName = "SampleInputOutputInt";

            StoredProcedureCommandResult result = _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntInputOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(outputParameterName, 1)
                .Execute();

            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
            Console.WriteLine("Row count: {0}", result.DataTable.Rows);
            Console.WriteLine("DataTable: {0}", result.DataTable.ToPrintFriendly());
        }

        /// <notes>
        /// Stored Procedures with Return parameters can retrieve them from result.ReturnParameters
        /// </notes>
        private void ExecuteStoredProcedureWithReturnParameter()
        {
            StoredProcedureCommandResult result = _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_NoInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .Execute();

            Console.WriteLine("Return parameter: {0}", result.GetReturnParameter<int>());
        }

        /// <notes>
        /// Stored Procedures with both Output parameters and a Return can be called from the command builder
        /// </notes>
        private void ExecuteStoredProcedureWithInputOutputAndReturn()
        {
            string outputParameterName = "SampleOutputBigInt";

            StoredProcedureCommandResult result = _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_Input_Output_ReturnBigInt]")
                .AddInputParameter("SampleTableID", 0)
                .AddInputOutputParameter(outputParameterName, 2)
                .WithReturnParameter()
                .Execute();

            Console.WriteLine("Output parameter: {0}", result.GetOutputParameter<int>(outputParameterName));
            Console.WriteLine("Return parameter: {0}", result.GetReturnParameter<int>());
        }

        /// <notes>
        /// Stored Procedures with optional parameters can be called
        /// </notes>
        private void ExecuteStoredProcedureWithOptionalInputParameter()
        {
            StoredProcedureCommandResult result = _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_OptionalInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .Execute();

            Console.WriteLine("Return parameter: {0}", result.GetReturnParameter<int>());
        }

        protected override void Init()
        {
            SampleMethods = new List<SampleMethod>
            {
                new SampleMethod("1", "ExecuteStoredProcedureWithAllInputTypesAndTableResult()", ExecuteStoredProcedureWithAllInputTypesAndTableResult),
                new SampleMethod("2", "ExecuteStoredProcedureWithOutput()", ExecuteStoredProcedureWithOutput),
                new SampleMethod("3", "ExecuteStoredProcedureWithInputMultipleOutputAndTableResult()", ExecuteStoredProcedureWithInputMultipleOutputAndTableResult),
                new SampleMethod("4", "ExecuteStoredProcedureWithInputOutputParameter()", ExecuteStoredProcedureWithInputOutputParameter),
                new SampleMethod("5", "ExecuteStoredProcedureWithReturnParameter()", ExecuteStoredProcedureWithReturnParameter),
                new SampleMethod("6", "ExecuteStoredProcedureWithInputOutputAndReturn()", ExecuteStoredProcedureWithInputOutputAndReturn),
                new SampleMethod("7", "ExecuteStoredProcedureWithOptionalInputParameter()", ExecuteStoredProcedureWithOptionalInputParameter)
            };
        }
    }
}
