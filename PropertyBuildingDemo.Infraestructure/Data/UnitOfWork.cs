using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using PropertyBuildingDemo.Application.Config;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Repositories;
using System.Collections;
using System.Data;

namespace PropertyBuildingDemo.Infrastructure.Data
{
    /// <summary>
    /// Represents a unit of work for managing database operations within the application.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PropertyBuildingContext _context;
        private readonly ISystemLogger _systemLogger;
        private Hashtable _repositories;
        private IDbContextTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="systemLogger">The system logger.</param>
        public UnitOfWork(PropertyBuildingContext context, ISystemLogger systemLogger)
        {
            _context = context;
            this._systemLogger = systemLogger;
        }

        /// <summary>
        /// Begins a new transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            _transaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// Completes the unit of work and saves changes to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="inFinishTransaction">Indicates whether to finish the transaction.</param>
        /// <returns>True if changes were saved successfully; otherwise, false.</returns>
        public async Task<bool> Complete(CancellationToken cancellationToken = default, bool inFinishTransaction = true)
        {
            var result = false;
            try
            {
                var rowsAffected = await _context.SaveChangesAsync(cancellationToken);

                if (_transaction != null && inFinishTransaction)
                    await _transaction.CommitAsync(cancellationToken);
                result = rowsAffected > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _systemLogger.LogMessage(ELoggingLevel.Warn, 
                    $"We have {ex.Entries.Count} conflict(s) with entities {(string.Join(", ", ex.Entries.Select(x => x.Metadata.Name)))}");
                foreach (var entry in ex.Entries)
                {

                    var databaseEntry = await entry.GetDatabaseValuesAsync(cancellationToken);
                    if (databaseEntry == null)
                    {
                        // The entity has been deleted by another user.
                        // this situation can be handled as needed, e.g., log it or take appropriate action.
                    }
                    else
                    {
                        // 
                        // TODO: Handle the the changes made by different users, so Merge CHANGES or take other actions
                    }
                }
            }
            catch (Exception exc)
            {
                _systemLogger.LogExceptionMessage(ELoggingLevel.Error, this, exc);
                if (_transaction != null)
                    await _transaction.RollbackAsync(cancellationToken);
                throw exc.InnerException ?? exc;
            }
            finally
            {
                if (_transaction != null && inFinishTransaction)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }

            return result;
        }

        /// <summary>
        /// Disposes of the unit of work.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }

        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>An instance of the generic entity repository.</returns>
        IGenericEntityRepository<TEntity> IUnitOfWork.GetRepository<TEntity>()
        {
            _repositories ??= new Hashtable();
            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (IGenericEntityRepository<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(BaseEntityRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            _repositories.Add(type, repositoryInstance);
            return (IGenericEntityRepository<TEntity>)_repositories[type];
        }
    }
}
