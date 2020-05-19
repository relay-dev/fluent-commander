namespace FluentCommander.Database
{
    public interface IDatabaseCommandFactory
    {
        TCommand Create<TCommand>() where TCommand : IDatabaseCommand;
    }
}
