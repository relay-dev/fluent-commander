using System;

namespace FluentCommander.Database
{
    public class DatabaseCommandFactory : IDatabaseCommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseCommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TCommand Create<TCommand>() where TCommand : IDatabaseCommand
        {
            return (TCommand)_serviceProvider.GetService(typeof(TCommand));
        }
    }
}
