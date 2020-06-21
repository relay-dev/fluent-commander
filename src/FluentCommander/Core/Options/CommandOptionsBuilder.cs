namespace FluentCommander.Core.Options
{
    public class CommandOptionsBuilder
    {
        private readonly DatabaseCommandRequest _request;

        public CommandOptionsBuilder(DatabaseCommandRequest request)
        {
            _request = request;
            _request.Options ??= new CommandOptions();
        }

        /// <summary>
        /// When set to true, this option will override the default behavior of SqlConnection.Open() to disable the ten second delay and automatic connection retries triggered by transient errors.
        /// </summary>
        public CommandOptionsBuilder OpenConnectionWithoutRetry(bool flag = true)
        {
            _request.Options.OpenConnectionWithoutRetry = flag;

            return this;
        }
    }

    public class CommandOptionsBuilder<TBuilder> where TBuilder : class
    {
        private readonly DatabaseCommandRequest _request;

        public CommandOptionsBuilder(DatabaseCommandRequest request)
        {
            _request = request;
            _request.Options ??= new CommandOptions();
        }

        /// <summary>
        /// When set to true, this option will override the default behavior of SqlConnection.Open() to disable the ten second delay and automatic connection retries triggered by transient errors.
        /// </summary>
        public TBuilder OpenConnectionWithoutRetry(bool flag = true)
        {
            _request.Options.OpenConnectionWithoutRetry = flag;

            return this as TBuilder;
        }
    }
}
