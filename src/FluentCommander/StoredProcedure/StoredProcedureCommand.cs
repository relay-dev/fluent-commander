using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.StoredProcedure
{
    public class StoredProcedureCommand : StoredProcedureCommandBuilder
    {
        private readonly IDatabaseCommander _databaseCommander;

        public StoredProcedureCommand(IDatabaseCommander databaseCommander)
            : base(new StoredProcedureRequest())
        {
            _databaseCommander = databaseCommander;
        }

        public override StoredProcedureResult Execute()
        {
            return _databaseCommander.ExecuteStoredProcedure(CommandRequest);
        }

        public override async Task<StoredProcedureResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _databaseCommander.ExecuteStoredProcedureAsync(CommandRequest, cancellationToken);
        }
    }
}