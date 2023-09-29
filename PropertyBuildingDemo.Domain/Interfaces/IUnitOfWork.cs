namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// Represents a unit of work interface for managing database transactions.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity for the repository.</typeparam>
        /// <returns>The repository for the specified entity type.</returns>
        IGenericEntityRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntityDb;

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task BeginTransaction(CancellationToken cancellationToken = default);

        /// <summary>
        /// Completes the database transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="inFinishTransaction">A flag indicating whether to finish the transaction.</param>
        /// <returns>A task representing the asynchronous operation and a boolean indicating the transaction completion result.</returns>
        Task<bool> Complete(CancellationToken cancellationToken = default, bool inFinishTransaction = true);
    }
}
