using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.StoredProcedure
{
    public class StoredProcedureCommand : StoredProcedureCommandBuilder
    {
        private readonly IDatabaseCommander _databaseCommander;

        public StoredProcedureCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public override StoredProcedureResult Execute()
        {
            CommandRequest.Parameters = Parameters;
            CommandRequest.Timeout = CommandTimeout;

            return _databaseCommander.ExecuteStoredProcedure(CommandRequest);
        }

        public override async Task<StoredProcedureResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            CommandRequest.Parameters = Parameters;
            CommandRequest.Timeout = CommandTimeout;

            return await _databaseCommander.ExecuteStoredProcedureAsync(CommandRequest, cancellationToken);
        }
    }
}