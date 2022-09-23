using System.Threading;
using System.Threading.Tasks;
using FluentCommander.StoredProcedure;

namespace FluentCommander.EntityFramework
{
    public interface IDatabaseEntityRequestHandler : IDatabaseRequestHandler
    {
        /// <summary>
        /// Executes a stored procedure against the database this <see cref="IDatabaseRequestHandler"/> is connected to
        /// </summary>
        /// <param name="request">The data needed to execute the stored procedure command</param>
        /// <returns>The result of the stored procedure</returns>
        public StoredProcedureResult<TEntity> ExecuteStoredProcedure<TEntity>(StoredProcedureRequest request) where TEntity : class;

        /// <summary>
        /// Executes a stored procedure against the database this <see cref="IDatabaseRequestHandler"/> is connected to
        /// </summary>
        /// <param name="request">The data needed to execute the stored procedure command</param>
        /// <param name="cancellationToken">The CancellationToken from the caller</param>
        /// <returns>The result of the stored procedure</returns>
        public Task<StoredProcedureResult<TEntity>> ExecuteStoredProcedureAsync<TEntity>(StoredProcedureRequest request, CancellationToken cancellationToken) where TEntity : class;
    }
}
