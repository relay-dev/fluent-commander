using FluentCommander;
using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication.SqlServer.Samples
{
    /// <notes>
    /// This sample class demonstrates how to build command for a scalar SQL statement
    /// </notes>
    [SampleFixture]
    public class ScalarCommandSamples : CommandSampleBase
    {
        private readonly IDatabaseCommander _databaseCommander;

        public ScalarCommandSamples(
            IDatabaseCommander databaseCommander,
            IConfiguration config)
            : base(config)
        {
            _databaseCommander = databaseCommander;
        }

        /// <notes>
        /// SQL queries with parameters can be parameterized for SQL Server to cache the execution plan and to avoid injection
        /// This method demonstrates how to query the database with inline SQL using input parameters
        /// </notes>
        [Sample(Key = "1")]
        public async Task ExecuteScalarWithInput()
        {
            bool result = await _databaseCommander.BuildCommand()
                .ForScalar<bool>("SELECT [SampleBit] FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1)
                .AddInputParameter("SampleVarChar", "Row 1")
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Result: {0}", result);
        }

        /// <notes>
        /// Input parameters require a database type parameter, which can often be inferred by looking at the type of the parameter value
        /// If that default behavior does not meet your needs, you can specify the database type of your input parameter using this variation of AddInputParameter()
        /// </notes>
        [Sample(Key = "2")]
        public async Task ExecuteScalarWithInputSpecifyingType()
        {
            DateTime result = await _databaseCommander.BuildCommand()
                .ForScalar<DateTime>("SELECT [SampleDateTime] FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
                .AddInputParameter("SampleTableID", 1, DbType.Int32)
                .AddInputParameter("SampleVarChar", "Row 1", DbType.String)
                .ExecuteAsync(new CancellationToken());

            Console.WriteLine("Result: {0}", result);
        }
    }
}
