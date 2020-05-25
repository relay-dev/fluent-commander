namespace FluentCommander.Database.Core
{
    public class DatabaseCommandRequest
    {
        /// <summary>
        /// Sets the timeout (in seconds) for one specific command request
        /// </summary>
        public int? TimeoutInSeconds { get; set; }
    }
}
