using Consolater;
using FluentCommander.StoredProcedure;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Samples.Commands
{
    /// <notes>
    /// This Sample class demonstrates how to build a stored procedure command using various combinations of input, output and return parameters
    /// To see the bodies of these Stored Procedures, navigate to the Resources folder and review the setup-*.sql files
    /// </notes>
    [ConsoleAppMenuItem]
    public class StoredProcedureSamples : ConsoleAppBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public StoredProcedureSamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// Stored Procedures can be called with various input parameter types. This stored procedure has output, which is found on the result object
        /// </notes>
        [ConsoleAppSelection(Key = "1")]
        public async Task ExecuteStoredProcedureWithAllInputTypesAndTableResult()
        {
            StoredProcedureResult result = await _databaseCommander.BuildCommand()
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

            int count = result.Count;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }

        /// <notes>
        /// Stored Procedures with output parameters need to call AddOutputParameter(), and retrieve the output from result.OutputParameters
        /// </notes>
        [ConsoleAppSelection(Key = "2")]
        public async Task ExecuteStoredProcedureWithOutput()
        {
            string outputParameterName = "SampleOutputInt";

            StoredProcedureResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntOutput_NoResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName, SqlDbType.Int)
                .ExecuteAsync(new CancellationToken());

            int outputParameter = result.GetOutputParameter<int>(outputParameterName);

            Console.WriteLine("Output parameter: {0}", outputParameter);
        }

        /// <notes>
        /// Stored Procedures with more complex signatures can be called. This stored procedure has input, output and it returns a DataTable with the result
        /// </notes>
        [ConsoleAppSelection(Key = "3")]
        public async Task ExecuteStoredProcedureWithInputMultipleOutputAndTableResult()
        {
            string outputParameterName1 = "SampleOutputInt";
            string outputParameterName2 = "SampleOutputVarChar";

            StoredProcedureResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_MultipleOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddOutputParameter(outputParameterName1, SqlDbType.Int)
                .AddOutputParameter(outputParameterName2, SqlDbType.VarChar, 1000)
                .Timeout(TimeSpan.FromSeconds(30))
                .ExecuteAsync(new CancellationToken());

            int outputParameter1 = result.GetOutputParameter<int>(outputParameterName1);
            string outputParameter2 = result.GetOutputParameter<string>(outputParameterName2);
            int count = result.Count;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Output parameter: {0}", outputParameter1);
            Console.WriteLine("Output parameter: {0}", outputParameter2);
            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }

        /// <notes>
        /// Stored Procedures with InputOutput parameters need to call AddInputOutputParameter(), and retrieve the output from result.OutputParameters
        /// </notes>
        [ConsoleAppSelection(Key = "4")]
        public async Task ExecuteStoredProcedureWithInputOutputParameter()
        {
            string inputOutputParameterName = "SampleInputOutputInt";

            StoredProcedureResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_IntInputOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(inputOutputParameterName, 1)
                .ExecuteAsync(new CancellationToken());

            int inputOutputParameter = result.GetOutputParameter<int>(inputOutputParameterName);

            Console.WriteLine("Output parameter: {0}", inputOutputParameter);
        }

        /// <notes>
        /// Stored Procedures with InputOutput parameters need to call AddInputOutputParameter(), and retrieve the output from result.OutputParameters
        /// </notes>
        [ConsoleAppSelection(Key = "5")]
        public async Task ExecuteStoredProcedureWithInputOutputParameterSpecifyingType()
        {
            string inputOutputParameterName = "SampleInputOutputVarChar";

            StoredProcedureResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_BigIntInput_VarCharOutput_TableResult]")
                .AddInputParameter("SampleTableID", 1)
                .AddInputOutputParameter(inputOutputParameterName, 1, SqlDbType.VarChar, 50)
                .ExecuteAsync(new CancellationToken());

            string inputOutputParameter = result.GetOutputParameter<string>(inputOutputParameterName);

            Console.WriteLine("Output parameter: {0}", inputOutputParameter);
        }

        /// <notes>
        /// If a Stored Procedures has a Return parameter, the command should call .WithReturnParameter() and the result has the following method that can retrieve the return parameter: result.GetReturnParameter():
        /// </notes>
        [ConsoleAppSelection(Key = "6")]
        public async Task ExecuteStoredProcedureWithReturnParameter()
        {
            StoredProcedureResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_NoInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

            int returnParameter = result.GetReturnParameter<int>();

            Console.WriteLine("Return parameter: {0}", returnParameter);
        }

        /// <notes>
        /// Stored Procedures with both Output parameters and a Return can be called from the command builder
        /// </notes>
        [ConsoleAppSelection(Key = "7")]
        public async Task ExecuteStoredProcedureWithInputOutputAndReturn()
        {
            string outputParameterName = "SampleOutputBigInt";

            StoredProcedureResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_Input_Output_ReturnBigInt]")
                .AddInputParameter("SampleTableID", 0)
                .AddInputOutputParameter(outputParameterName, 2)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

            int outputParameter = result.GetOutputParameter<int>(outputParameterName);
            int returnParameter = result.GetReturnParameter<int>();

            Console.WriteLine("Output parameter: {0}", outputParameter);
            Console.WriteLine("Return parameter: {0}", returnParameter);
        }

        /// <notes>
        /// Stored Procedures with optional parameters can be called
        /// </notes>
        [ConsoleAppSelection(Key = "8")]
        public async Task ExecuteStoredProcedureWithOptionalInputParameter()
        {
            StoredProcedureResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_OptionalInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1)
                .WithReturnParameter()
                .ExecuteAsync(new CancellationToken());

            int returnParameter = result.GetReturnParameter<int>();

            Console.WriteLine("Return parameter: {0}", returnParameter);
        }

        /// <notes>
        /// Input parameters require a database type parameter, which can often be inferred by looking at the type of the parameter value
        /// If that default behavior does not meet your needs, you can specify the database type of your input parameter using this variation of AddInputParameter()
        /// </notes>
        [ConsoleAppSelection(Key = "9")]
        public async Task ExecuteStoredProcedureWithInputSpecifyingType()
        {
            StoredProcedureResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_OptionalInput_NoOutput_ReturnInt]")
                .AddInputParameter("SampleTableID", 1, SqlDbType.BigInt)
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }

        /// <notes>
        /// SqlDataReader behaviors are exposed
        /// </notes>
        [ConsoleAppSelection(Key = "10")]
        public async Task ExecuteStoredProcedureWithBehaviors()
        {
            StoredProcedureResult result = await _databaseCommander.BuildCommand()
                .ForStoredProcedure("[dbo].[usp_VarCharInput_NoOutput_TableResult]")
                .AddInputParameter("SampleVarChar", "Row 1", SqlDbType.VarChar, 1000)
                .Behaviors(behavior => behavior.SingleResult().KeyInfo())
                .ExecuteAsync(new CancellationToken());

            int count = result.Count;
            bool hasData = result.HasData;
            DataTable dataTable = result.DataTable;

            Console.WriteLine("Row count: {0}", count);
            Console.WriteLine("Has Data: {0}", hasData);
            Console.WriteLine("DataTable: {0}", Print(dataTable));
        }
    }
}
