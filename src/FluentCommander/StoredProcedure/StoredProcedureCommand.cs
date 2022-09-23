using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.StoredProcedure
{
    public class StoredProcedureCommand : StoredProcedureCommandBuilder
    {
        private readonly IDatabaseRequestHandler _databaseRequestHandler;

        public StoredProcedureCommand(IDatabaseRequestHandler databaseRequestHandler)
            : base(new StoredProcedureRequest())
        {
            _databaseRequestHandler = databaseRequestHandler;
        }

        public override StoredProcedureResult Execute()
        {
            return _databaseRequestHandler.ExecuteStoredProcedure(CommandRequest);
        }

        public override async Task<StoredProcedureResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _databaseRequestHandler.ExecuteStoredProcedureAsync(CommandRequest, cancellationToken);
        }
    }
}