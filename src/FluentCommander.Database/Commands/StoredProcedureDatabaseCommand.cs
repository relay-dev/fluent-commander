using System.Collections.Generic;
using System.Data;
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
            List<DatabaseCommandParameter> p = Parameters;

            DataTable dataTable = _databaseCommander.ExecuteStoredProcedure(_storedProcedureName, ref p);

            return new StoredProcedureCommandResult(Parameters, dataTable);
        }

        /// <summary>
        /// This method does not return the Output or Return parameters because the parameters (p) cannot be passed by reference to an async method
        /// There are ways around this for a future implementation
        /// </summary>
        public override async Task<StoredProcedureCommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            List<DatabaseCommandParameter> p = Parameters;

            DataTable dataTable = await _databaseCommander.ExecuteStoredProcedureAsync(_storedProcedureName, cancellationToken, p);

            return new StoredProcedureCommandResult(Parameters, dataTable);
        }
    }
}