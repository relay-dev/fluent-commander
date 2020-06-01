using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Core.CommandBuilders;

namespace FluentCommander.StoredProcedure
{
    public class StoredProcedureCommand : ParameterizedCommandBuilder<StoredProcedureCommand, StoredProcedureResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly StoredProcedureRequest _storedProcedureRequest;

        public StoredProcedureCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
            _storedProcedureRequest = new StoredProcedureRequest();
        }

        public StoredProcedureCommand Name(string storedProcedureName)
        {
            _storedProcedureRequest.StoredProcedureName = storedProcedureName;

            return this;
        }

        public override StoredProcedureResult Execute()
        {
            _storedProcedureRequest.DatabaseParameters = Parameters;
            _storedProcedureRequest.Timeout = CommandTimeout;

            return _databaseCommander.ExecuteStoredProcedure(_storedProcedureRequest);
        }

        public override async Task<StoredProcedureResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            _storedProcedureRequest.DatabaseParameters = Parameters;
            _storedProcedureRequest.Timeout = CommandTimeout;

            return await _databaseCommander.ExecuteStoredProcedureAsync(_storedProcedureRequest, cancellationToken);
        }
    }
}