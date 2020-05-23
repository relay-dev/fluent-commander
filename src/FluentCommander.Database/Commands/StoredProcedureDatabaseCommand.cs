using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public class StoredProcedureDatabaseCommand : ParameterizedDatabaseCommand<StoredProcedureCommandResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private string _storedProcedureName;

        public StoredProcedureDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public StoredProcedureDatabaseCommand Named(string storedProcedureName)
        {
            _storedProcedureName = storedProcedureName;

            return this;
        }

        public override StoredProcedureCommandResult Execute()
        {
            StoredProcedureResult result = _databaseCommander.ExecuteStoredProcedure(_storedProcedureName, Parameters);

            return new StoredProcedureCommandResult(result.Parameters, result.DataTable);
        }

        public override async Task<StoredProcedureCommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            StoredProcedureResult result = await _databaseCommander.ExecuteStoredProcedureAsync(_storedProcedureName, cancellationToken, Parameters);

            return new StoredProcedureCommandResult(result.Parameters, result.DataTable);
        }
    }
}