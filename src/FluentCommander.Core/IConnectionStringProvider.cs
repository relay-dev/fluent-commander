namespace FluentCommander.Core
{
    public interface IConnectionStringProvider
    {
        string Get(string connectionStringName);
    }
}
