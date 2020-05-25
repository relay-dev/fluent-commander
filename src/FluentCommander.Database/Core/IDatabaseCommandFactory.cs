namespace FluentCommander.Database.Core
{
    public interface IDatabaseCommandFactory
    {
        /// <summary>
        /// Creates an IDatabaseCommand
        /// </summary>
        /// <typeparam name="TCommand">The type of command to create</typeparam>
        /// <returns>The command instance</returns>
        TCommand Create<TCommand>() where TCommand : IDatabaseCommand;
    }
}
