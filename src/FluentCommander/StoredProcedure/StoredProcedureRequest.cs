using FluentCommander.Core;
using FluentCommander.Core.Behaviors;

namespace FluentCommander.StoredProcedure
{
    public class StoredProcedureRequest : ParameterizedCommandRequest, IHaveReadBehaviors
    {
        public StoredProcedureRequest() { }

        public StoredProcedureRequest(string storedProcedureName)
        {
            StoredProcedureName = storedProcedureName;
        }

        /// <summary>
        /// The name of the stored procedure to be executed
        /// </summary>
        public string StoredProcedureName { get; set; }

        /// <summary>
        /// The behaviors to command should follow
        /// </summary>
        public ReadBehaviors ReadBehaviors { get; set; }
    }
}
