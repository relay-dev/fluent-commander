using Consolater;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Samples.Commands
{
    /// <notes>
    /// This sample class demonstrates how to build command for a scalar SQL statement
    /// </notes>
    [ConsoleAppMenuItem]
    public class ScalarSamples : ConsoleAppBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public ScalarSamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// SQL Scalar queries can be parameterized
        /// </notes>
        [ConsoleAppSelection(Key = "1")]
        public async Task ExecuteScalarWithInput()
        {
            bool result = await _databaseCommander.BuildCommand()
                .ForScalar<bool>("SELECT [SampleBit] FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Data: {0}", result);
        }

        /// <notes>
        /// If the default behavior does not meet your needs, you can specify the database type of your input parameter using this variation of AddInputParameter()
        /// </notes>
        [ConsoleAppSelection(Key = "2")]
        public async Task ExecuteScalarWithInputSpecifyingType()
        {
            DateTime result = await _databaseCommander.BuildCommand()
                .ForScalar<DateTime>("SELECT [SampleDateTime] FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1, SqlDbType.Int)
                .AddInputParameter("SampleVarChar", "Row 1", SqlDbType.VarChar)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Data: {0}", result);
        }
    }
}
