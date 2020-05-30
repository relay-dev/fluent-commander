using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Commands.Builders;

namespace FluentCommander.Commands
{
    public class StoredProcedureCommand : ParameterizedCommandBuilder<StoredProcedureCommand, StoredProcedureResult>
    {
        protected readonly IDatabaseCommander DatabaseCommander;
        protected readonly StoredProcedureRequest StoredProcedureRequest;

        public StoredProcedureCommand(IDatabaseCommander databaseCommander)
        {
            DatabaseCommander = databaseCommander;
            StoredProcedureRequest = new StoredProcedureRequest();
        }

        public StoredProcedureCommand Name(string storedProcedureName)
        {
            StoredProcedureRequest.StoredProcedureName = storedProcedureName;

            return this;
        }

        public override StoredProcedureResult Execute()
        {
            StoredProcedureRequest.DatabaseParameters = Parameters;
            StoredProcedureRequest.Timeout = CommandTimeout;

            return DatabaseCommander.ExecuteStoredProcedure(StoredProcedureRequest);
        }

        public override async Task<StoredProcedureResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            StoredProcedureRequest.DatabaseParameters = Parameters;
            StoredProcedureRequest.Timeout = CommandTimeout;

            return await DatabaseCommander.ExecuteStoredProcedureAsync(StoredProcedureRequest, cancellationToken);
        }
    }
}