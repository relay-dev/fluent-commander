using System.Data;

namespace FluentCommander
{
    public interface IDatabaseCommanderTransactionFactory
    {
        IDbTransaction Create(string transactionName = null, IsolationLevel? iso = null);
    }
}
